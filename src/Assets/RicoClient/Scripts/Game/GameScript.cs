using Newtonsoft.Json;
using RicoClient.Scripts.Cards;
using RicoClient.Scripts.Cards.Entities;
using RicoClient.Scripts.Exceptions;
using RicoClient.Scripts.Game.CardLogic.BoardLogic;
using RicoClient.Scripts.Game.CardLogic.CurrentLogic;
using RicoClient.Scripts.Game.CardLogic.HandLogic;
using RicoClient.Scripts.Game.InGameDeck;
using RicoClient.Scripts.Network.Entities.Websocket;
using System;
using System.Collections.Generic;
using TMPro;
using UniRx.Async;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace RicoClient.Scripts.Game
{
    public enum GameState
    {
        Init,
        Start,
        MyTurn,
        MyTurnFinish,
        EnemyTurn,
        EnemyTurnFinish,
        Finished
    }

    public class GameScript : MonoBehaviour
    {
        private const string EndTurnText = "End turn";
        private const string WaitTurnText = "Waiting";
        private const string EnemyPlayingText = "Playing";

        private CardsManager _cards;
        private GameManager _game;

        [SerializeField]
        private GameObject _screenOverlayCanvas = null;

        [Header("Nicknames")]
        [SerializeField]
        private TMP_Text _myName = null;
        [SerializeField]
        private TMP_Text _enemyName = null;

        [Header("Cards Prefabs")]
        [SerializeField]
        private GameObject _backCard = null;
        [SerializeField]
        private UnitCardScript _unitCard = null;
        [SerializeField]
        private BuildingCardScript _buildingCard = null;
        [SerializeField]
        private SpellCardScript _spellCard = null;

        [Header("Decks")]
        [SerializeField]
        private InGameDeckScript _myDeck = null;
        [SerializeField]
        private InGameDeckScript _enemyDeck = null;

        [Header("Hands")]
        [SerializeField]
        private HandScript _myHand = null;
        [SerializeField]
        private HandScript _enemyHand = null;

        [Header("Boards")]
        [SerializeField]
        private BoardScript _myBoard = null;
        [SerializeField]
        private BoardScript _enemyBoard = null;

        [Header("Bases")]
        [SerializeField]
        private BaseBuildingScript _myBase = null;
        [SerializeField]
        private BaseBuildingScript _enemyBase = null;

        [Header("Resource Storages")]
        [SerializeField]
        private TMP_Text _myResources = null;
        [SerializeField]
        private TMP_Text _enemyResources = null;

        [Header("Turn Statuses")]
        [SerializeField]
        private TMP_Text _myStatus = null;
        [SerializeField]
        private TMP_Text _enemyStatus = null;
        [SerializeField]
        private Button _endTurn = null;

        public int MyResources { get { return int.Parse(_myResources.text); } }
        public int EnemyResources { get { return int.Parse(_enemyResources.text); } }

        public static GameState State { get; private set; }

        private BaseCardScript _currentAbilityCard;
        private UnitCardScript _currentAttackingCard;

        [Inject]
        public void Initialize(CardsManager cards, GameManager game)
        {
            _cards = cards;
            _game = game;

            State = GameState.Init;

            MyHandCardLogic.OnCardSelected += CardSelected;
            MyCurrentCardLogic.OnCardReturnedToHand += CardDeselected;
            MyCurrentCardLogic.OnCardDroppedToBoard += CardDropped;
            MyBoardCardLogic.OnCardPrepAttack += CardTakenToAttack;
            MyBoardCardLogic.OnCardUnprepAttack += CardReleasedFromAttack;
            MyBoardCardLogic.OnWarcryCheck += ActivateWarcryAbility;
            BaseBoardLogic.OnBoardCardEnter += AimChoosed;
            BaseBoardLogic.OnBoardCardExit += AimDechosed;
            BaseBoardLogic.OnDroppedOnCard += CardBeenChoosedForAction;
            BaseBoardLogic.OnClickedOnCard += CardBeenChoosedForAction;

            BaseBuildingScript.OnBaseEnter += AimChoosed;
            BaseBuildingScript.OnBaseExit += AimDechosed;
            BaseBuildingScript.OnOnDropped += BaseBeenChoosedForAction;
            BaseBuildingScript.OnClicked += BaseBeenChoosedForAction;

            _game.OnWebsocketMessage += OnWebsocketMessage;
            _game.OnWebsocketError += OnWebsocketError;
        }

        protected async void Start()
        {
            if (State != GameState.Init)
                throw new GameCoreException("Not initial game!");

            State = GameState.Start;

            _myName.text = _game.StartValues.MyNickname;
            _enemyName.text = _game.StartValues.EnemyNickname;

            _myResources.text = "0";
            _enemyResources.text = "0";

            _myStatus.text = WaitTurnText;
            _enemyStatus.text = WaitTurnText;
            _endTurn.interactable = false;

            _myDeck.SetDeck(_game.StartValues.PlayerDeckInitSize);
            _enemyDeck.SetDeck(_game.StartValues.EnemyDeckInitSize);

            _myBase.FillBase(_game.StartValues.MyBase);
            _enemyBase.FillBase(_game.StartValues.EnemyBase);

            TakeMyCards(_game.StartValues.MyStartCards);
            TakeEnemyCards(_game.StartValues.EnemyStartCardsCount);

            await _game.SendReadyMessage();
        }

        protected void OnDestroy()
        {
            MyHandCardLogic.OnCardSelected -= CardSelected;
            MyCurrentCardLogic.OnCardReturnedToHand -= CardDeselected;
            MyCurrentCardLogic.OnCardDroppedToBoard -= CardDropped;
            MyBoardCardLogic.OnCardPrepAttack -= CardTakenToAttack;
            MyBoardCardLogic.OnCardUnprepAttack -= CardReleasedFromAttack;
            MyBoardCardLogic.OnWarcryCheck -= ActivateWarcryAbility;
            BaseBoardLogic.OnBoardCardEnter -= AimChoosed;
            BaseBoardLogic.OnBoardCardExit -= AimDechosed;
            BaseBoardLogic.OnDroppedOnCard -= CardBeenChoosedForAction;
            BaseBoardLogic.OnClickedOnCard -= CardBeenChoosedForAction;

            BaseBuildingScript.OnBaseEnter -= AimChoosed;
            BaseBuildingScript.OnBaseExit -= AimDechosed;
            BaseBuildingScript.OnOnDropped -= BaseBeenChoosedForAction;
            BaseBuildingScript.OnClicked -= BaseBeenChoosedForAction;

            _game.OnWebsocketMessage -= OnWebsocketMessage;
            _game.OnWebsocketError -= OnWebsocketError;
        }

        public void MyTurnStart(List<TakenCardPayload> takenCards)
        {
            if (State != GameState.EnemyTurnFinish && State != GameState.Start)
                throw new GameCoreException("Wrong game state before player turn start!");
            State = GameState.MyTurn;

            // Update player resources
            int generatedResources = GenerateResources(_myBoard.OnBoardCards, _myBase);
            _myResources.text = (MyResources + generatedResources).ToString();

            TakeMyCards(takenCards);

            foreach (var onBoardCard in _myBoard.OnBoardCards)
            {
                MyBoardCardLogic cardLogic = (MyBoardCardLogic) onBoardCard.GetComponentInChildren<BaseCardScript>().Logic;
                cardLogic.CanAttack = true;
            }

            _myStatus.text = EndTurnText;
            _endTurn.interactable = true;
        }

        public void MyTurnFinish()
        {
            if (State != GameState.MyTurn)
                throw new GameCoreException("Wrong game state before player turn finish!");
            State = GameState.MyTurnFinish;

            _myStatus.text = WaitTurnText;
            _endTurn.interactable = false;
        }

        public void EnemyTurnStart(int takenCards)
        {
            if (State != GameState.MyTurnFinish && State != GameState.Start)
                throw new GameCoreException("Wrong game state before enemy turn start!");
            State = GameState.EnemyTurn;

            // Update enemy resources
            int generatedResources = GenerateResources(_enemyBoard.OnBoardCards, _enemyBase);
            _enemyResources.text = (int.Parse(_enemyResources.text) + generatedResources).ToString();

            TakeEnemyCards(takenCards);

            _enemyStatus.text = EnemyPlayingText;
        }

        public void EnemyTurnFinish()
        {
            if (State != GameState.EnemyTurn)
                throw new GameCoreException("Wrong game state before enemy turn finish!");
            State = GameState.EnemyTurnFinish;

            _enemyStatus.text = WaitTurnText;
        }

        public void MyCardPlayed(WSResponse resp)
        {
            CardPlayResponse payload = JsonConvert.DeserializeObject<CardPlayResponse>(resp.Payload.ToString());
            if (!payload.Approved)  // If not approved means some cheat attempt or net bug. How resolve?
            {
                Debug.LogError(resp.Error);
            }
        }   
        
        public void EnemyCardPlayed(EnemyCardPlayPayload data)
        {
            _enemyHand.PlayCardFromHand();

            Card card = _cards.GetCardById(data.CardId);
            _enemyBoard.AddEnemyCardOnBoard(ConvertTakenCardToScript(card, data.DeckCardId));
            _enemyResources.text = (EnemyResources - card.Cost).ToString();
        }

        public void BattlePhase()
        {
            
        }

        public async void OnEndTurnClick()
        {
            await _game.SendEndTurnMessage();
        }

        public async void OnExitClick()
        {
            await _game.CloseSocket();

            SceneManager.LoadSceneAsync("RicoClient/Scenes/MenuScene");
        }

        private void OnWebsocketMessage(WSResponse msg)
        {
            switch (msg.Type)
            {
                case ResponseCommandType.PlayersTurnStart:
                    PlayerStartTurnPayload myTurnStartData = JsonConvert.DeserializeObject<PlayerStartTurnPayload>(msg.Payload.ToString());
                    MyTurnStart(myTurnStartData.PlayerTakenCards);
                    break;
                case ResponseCommandType.EnemyTurnStart:
                    EnemyStartTurnPayload enemyTurnStartData = JsonConvert.DeserializeObject<EnemyStartTurnPayload>(msg.Payload.ToString());
                    EnemyTurnStart(enemyTurnStartData.EnemyTakenCards);
                    break;
                case ResponseCommandType.PlayersTurnFinsh:
                    MyTurnFinish();
                    break;
                case ResponseCommandType.EnemyTurnFinsh:
                    EnemyTurnFinish();
                    break;
                case ResponseCommandType.CardPlayResponse:
                    MyCardPlayed(msg);
                    break;
                case ResponseCommandType.EnemyCardPlayResponse:
                    EnemyCardPlayPayload enemyCardPlayData = JsonConvert.DeserializeObject<EnemyCardPlayPayload>(msg.Payload.ToString());
                    EnemyCardPlayed(enemyCardPlayData);
                    break;
            }
        }

        private void OnWebsocketError(string error)
        {
            Debug.LogError($"An error has occurred during game: {error}!");
        }

        private void TakeMyCards(List<TakenCardPayload> cards)
        {
            foreach (var startCard in cards)
            {
                _myDeck.TakeCard();

                Card card = _cards.GetCardById(startCard.CardId);
                var convertedCard = ConvertTakenCardToScript(card, startCard.CardDeckId);

                _myHand.AddRevealedCardInHand(convertedCard);
            }
        }

        private void TakeEnemyCards(int countCards)
        {
            for (int i = 0; i < countCards; i++)
            {
                _enemyDeck.TakeCard();

                var hiddenCard = Instantiate(_backCard);

                _enemyHand.AddHiddenCardInHand(hiddenCard);
            }
        }    

        private void CardSelected(BaseCardScript card)
        {
            if (card.Cost > MyResources)
                return;

            card.Select(_screenOverlayCanvas.transform);
            _myHand.BlockHand();

            if (!(card is SpellCardScript))
            {
                _myBoard.HighlightAreaWith(card);
            }
            else
            {

            }
        }

        private void CardDeselected(BaseCardScript card)
        {
            _myBoard.RemoveHighlight();
            _myHand.EnableHand();
        }

        private async void CardDropped(GameObject inHandHolder, BaseCardScript card)
        {
            _myResources.text = (MyResources - card.Cost).ToString();

            _myBoard.RemoveHighlight();
            Destroy(inHandHolder);
            _myHand.RecalculateSpacing(1);
            _myHand.EnableHand();

            await _game.SendPlacedCardMessage(card.DeckCardId);
        }

        private void ActivateWarcryAbility(BaseCardScript cardScript)
        {
            Card card = _cards.GetCardById(cardScript.CardId);

            if (card.Ability == null)
                return;

            if (card.Ability.Activation != AbilityActivationType.Warcry)
                return;

            int highlitedCards = 0;
            if (card.Ability.Target.HasFlag(AbilityTargetType.Ally))  // Is friend
            {
                foreach (var myBoardCard in _myBoard.OnBoardCards)
                {
                    if (card.Ability.TargetType.HasFlag(CardType.Unit))
                    {
                        UnitCardScript unitCard = myBoardCard.GetComponentInChildren<UnitCardScript>();
                        if (unitCard != null)
                        {
                            BaseBoardLogic cardLogic = (BaseBoardLogic) unitCard.Logic;
                            cardLogic.HighlightCard();
                            highlitedCards++;
                        }
                    }
                    
                    if (card.Ability.TargetType.HasFlag(CardType.Building))
                    {
                        BuildingCardScript buildingCard = myBoardCard.GetComponentInChildren<BuildingCardScript>();
                        if (buildingCard != null)
                        {
                            BaseBoardLogic cardLogic = (BaseBoardLogic) buildingCard.Logic;
                            cardLogic.HighlightCard();
                            highlitedCards++;
                        }
                    }
                }

                if (card.Ability.TargetType.HasFlag(CardType.Base))
                {
                    _myBase.Highlight();
                    highlitedCards++;
                }
            }
            
            if (card.Ability.Target.HasFlag(AbilityTargetType.Enemy))  // Is enemy
            {
                foreach (var enemyBoardCard in _enemyBoard.OnBoardCards)
                {
                    if (card.Ability.TargetType.HasFlag(CardType.Unit))
                    {
                        UnitCardScript unitCard = enemyBoardCard.GetComponentInChildren<UnitCardScript>();
                        if (unitCard != null)
                        {
                            BaseBoardLogic cardLogic = (BaseBoardLogic) unitCard.Logic;
                            cardLogic.HighlightCard();
                            highlitedCards++;
                        }
                    }

                    if (card.Ability.TargetType.HasFlag(CardType.Building))
                    {
                        BuildingCardScript buildingCard = enemyBoardCard.GetComponentInChildren<BuildingCardScript>();
                        if (buildingCard != null)
                        {
                            BaseBoardLogic cardLogic = (BaseBoardLogic) buildingCard.Logic;
                            cardLogic.HighlightCard();
                            highlitedCards++;
                        }
                    }
                }

                if (card.Ability.TargetType.HasFlag(CardType.Base))
                {
                    _enemyBase.Highlight();
                    highlitedCards++;
                }
            }

            if (highlitedCards > 0)
            {
                ((MyBoardCardLogic) cardScript.Logic).ActivateDirectedAbility();
                _currentAbilityCard = cardScript;
            }
            else
            {
                // send none
            }
        }

        private void CardTakenToAttack(UnitCardScript unit)
        {
            foreach (var enemy in _enemyBoard.OnBoardCards)
            {
                BaseCardScript enemyCard = enemy.GetComponentInChildren<BaseCardScript>();
                EnemyBoardCardLogic cardLogic = (EnemyBoardCardLogic) enemyCard.Logic;
                cardLogic.HighlightCard();
            }

            _currentAttackingCard = unit;
            _enemyBase.Highlight();
        }

        private void CardReleasedFromAttack(UnitCardScript unit)
        {
            foreach (var enemy in _enemyBoard.OnBoardCards)
            {
                BaseCardScript enemyCard = enemy.GetComponentInChildren<BaseCardScript>();
                EnemyBoardCardLogic cardLogic = (EnemyBoardCardLogic) enemyCard.Logic;
                cardLogic.UnhighlightCard();
            }

            _currentAttackingCard = null;
            _enemyBase.Unhighlight();
        }

        private void AimChoosed(Vector3 targetDownSide)
        {
            // Mean we attaking by our card
            if (_currentAttackingCard != null)
            {
                ((MyBoardCardLogic) _currentAttackingCard.Logic).SetAimTarget(targetDownSide);
            }
            else if (_currentAbilityCard != null)  // Mean we aiming with ability or spell card
            {
                ((MyBoardCardLogic) _currentAbilityCard.Logic).SetAimTarget(targetDownSide);
            }
        }

        private void AimDechosed()
        {
            if (_currentAttackingCard != null)
            {
                ((MyBoardCardLogic) _currentAttackingCard.Logic).RemoveAimTarget();
            }
            else if (_currentAbilityCard != null)
            {
                ((MyBoardCardLogic) _currentAbilityCard.Logic).RemoveAimTarget();
            }
        }

        private void CardBeenChoosedForAction(BaseCardScript card)
        {
            if (_currentAttackingCard != null)
            {
                EntityCardScript entity = (EntityCardScript) card;
                entity.Damage(_currentAttackingCard.Attack);
                _currentAttackingCard.Damage(entity.Attack);

                MyBoardCardLogic attackingLogic = (MyBoardCardLogic) _currentAttackingCard.Logic;
                attackingLogic.RemoveAimTarget();
                attackingLogic.CanAttack = false;
            }
            else if (_currentAbilityCard != null)
            {
                Card currentCard = _cards.GetCardById(_currentAbilityCard.CardId);

                if (currentCard.Ability.TargetCount == 1)
                {
                    // Send info about

                    EntityCardScript entity = (EntityCardScript) card;
                    if (currentCard.Ability.Type == AbilityType.Damage)
                    {
                        entity.ShiftStats(-currentCard.Ability.Health, -currentCard.Ability.Attack);
                    }
                    else if (currentCard.Ability.Type == AbilityType.Buff)
                    {
                        entity.ShiftStats(currentCard.Ability.Health, currentCard.Ability.Attack);
                    }
                }
                else if (currentCard.Ability.TargetCount == -1)
                {
                    // Send info about

                    if (currentCard.Ability.Target.HasFlag(AbilityTargetType.Ally))  // If friend can be damaged
                    {
                        foreach (var myBoardCard in _myBoard.OnBoardCards)
                        {
                            if (currentCard.Ability.TargetType.HasFlag(CardType.Unit))  // If unit can be damaged
                            {
                                UnitCardScript unitCard = myBoardCard.GetComponentInChildren<UnitCardScript>();
                                if (unitCard != null)
                                {
                                    if (currentCard.Ability.Type == AbilityType.Damage)
                                    {
                                        unitCard.ShiftStats(-currentCard.Ability.Health, -currentCard.Ability.Attack);
                                    }
                                    else if (currentCard.Ability.Type == AbilityType.Buff)
                                    {
                                        unitCard.ShiftStats(currentCard.Ability.Health, currentCard.Ability.Attack);
                                    }
                                }
                            }

                            if (currentCard.Ability.TargetType.HasFlag(CardType.Building))   // If building can be damaged
                            {
                                BuildingCardScript buildingCard = myBoardCard.GetComponentInChildren<BuildingCardScript>();
                                if (buildingCard != null)
                                {
                                    if (currentCard.Ability.Type == AbilityType.Damage)
                                    {
                                        buildingCard.ShiftStats(-currentCard.Ability.Health, -currentCard.Ability.Attack);
                                    }
                                    else if (currentCard.Ability.Type == AbilityType.Buff)
                                    {
                                        buildingCard.ShiftStats(currentCard.Ability.Health, currentCard.Ability.Attack);
                                    }
                                }
                            }
                        }

                        if (currentCard.Ability.TargetType.HasFlag(CardType.Base))   // If base can be damaged
                        {
                            if (currentCard.Ability.Type == AbilityType.Damage)
                            {
                                _myBase.ShiftStats(-currentCard.Ability.Health);
                            }
                            else if (currentCard.Ability.Type == AbilityType.Buff)
                            {
                                _myBase.ShiftStats(currentCard.Ability.Health);
                            }
                        }
                    }

                    if (currentCard.Ability.Target.HasFlag(AbilityTargetType.Enemy))
                    {
                        foreach (var enemyBoardCard in _enemyBoard.OnBoardCards)
                        {
                            if (currentCard.Ability.TargetType.HasFlag(CardType.Unit))
                            {
                                UnitCardScript unitCard = enemyBoardCard.GetComponentInChildren<UnitCardScript>();
                                if (unitCard != null)
                                {
                                    if (currentCard.Ability.Type == AbilityType.Damage)
                                    {
                                        unitCard.ShiftStats(-currentCard.Ability.Health, -currentCard.Ability.Attack);
                                    }
                                    else if (currentCard.Ability.Type == AbilityType.Buff)
                                    {
                                        unitCard.ShiftStats(currentCard.Ability.Health, currentCard.Ability.Attack);
                                    }
                                }
                            }

                            if (currentCard.Ability.TargetType.HasFlag(CardType.Building))
                            {
                                BuildingCardScript buildingCard = enemyBoardCard.GetComponentInChildren<BuildingCardScript>();
                                if (buildingCard != null)
                                {
                                    if (currentCard.Ability.Type == AbilityType.Damage)
                                    {
                                        buildingCard.ShiftStats(-currentCard.Ability.Health, -currentCard.Ability.Attack);
                                    }
                                    else if (currentCard.Ability.Type == AbilityType.Buff)
                                    {
                                        buildingCard.ShiftStats(currentCard.Ability.Health, currentCard.Ability.Attack);
                                    }
                                }
                            }
                        }

                        if (currentCard.Ability.TargetType.HasFlag(CardType.Base))
                        {
                            if (currentCard.Ability.Type == AbilityType.Damage)
                            {
                                _enemyBase.ShiftStats(-currentCard.Ability.Health);
                            }
                            else if (currentCard.Ability.Type == AbilityType.Buff)
                            {
                                _enemyBase.ShiftStats(currentCard.Ability.Health);
                            }
                        }
                    }
                }

                // Remove every highlight and aim line
                MyBoardCardLogic abilityLogic = (MyBoardCardLogic) _currentAbilityCard.Logic;
                abilityLogic.RemoveAimTarget();
                abilityLogic.DeactivateDirectedAbility();

                foreach (var myOnBoardCard in _myBoard.OnBoardCards)
                {
                    BaseBoardLogic cardLogic = (BaseBoardLogic) myOnBoardCard.GetComponentInChildren<BaseCardScript>().Logic;
                    cardLogic.UnhighlightCard();
                }
                foreach (var enemyOnBoardCard in _enemyBoard.OnBoardCards)
                {
                    BaseBoardLogic cardLogic = (BaseBoardLogic)enemyOnBoardCard.GetComponentInChildren<BaseCardScript>().Logic;
                    cardLogic.UnhighlightCard();
                }
                _myBase.Unhighlight();
                _enemyBase.Unhighlight();

                // Check for death

            }
        }

        private void BaseBeenChoosedForAction(BaseBuildingScript baseBuilding)
        {
            if (_currentAttackingCard != null)
            {
                baseBuilding.Damage(_currentAttackingCard.Attack);

                MyBoardCardLogic attackingLogic = (MyBoardCardLogic) _currentAttackingCard.Logic;
                attackingLogic.RemoveAimTarget();
                attackingLogic.CanAttack = false;
            }
            else if (_currentAbilityCard != null)
            {
                // Base
            }
        }

        private void ApplyAbilityOnEverybody()
        {

        }

        private int GenerateResources(List<GameObject> boardCards, BaseBuildingScript baseBuilding)
        {
            int resourceCount = baseBuilding.Resource;

            foreach (var onBoardCard in boardCards)
            {
                BuildingCardScript building = onBoardCard.GetComponentInChildren<BuildingCardScript>();
                if (building != null)
                    resourceCount += building.Resource;
            }

            return resourceCount;
        }

        private BaseCardScript ConvertTakenCardToScript(Card card, int deckCardId)
        {
            Debug.Log($"{card.CardId} with {deckCardId}");

            BaseCardScript res;

            switch (card.Type)
            {
                case CardType.Unit:
                    res = Instantiate(_unitCard.gameObject).GetComponent<UnitCardScript>();          
                    break;
                case CardType.Building:
                    res = Instantiate(_buildingCard.gameObject).GetComponent<BuildingCardScript>();
                    break;
                case CardType.Spell:
                    res = Instantiate(_spellCard.gameObject).GetComponent<SpellCardScript>();
                    break;
                default:
                    throw new NotSupportedException($"Some not supported cards type ({card.Type}) arrived!");
            }

            res.FillCard(card, deckCardId);
            return res;
        }
    }
}
