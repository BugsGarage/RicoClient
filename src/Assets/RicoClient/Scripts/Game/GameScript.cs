using Newtonsoft.Json;
using RicoClient.Scripts.Cards;
using RicoClient.Scripts.Cards.Entities;
using RicoClient.Scripts.Exceptions;
using RicoClient.Scripts.Game.CardLogic.BoardLogic;
using RicoClient.Scripts.Game.CardLogic.CurrentLogic;
using RicoClient.Scripts.Game.CardLogic.HandLogic;
using RicoClient.Scripts.Game.InGameDeck;
using RicoClient.Scripts.Menu.Modals;
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
        private const string WinnerText = "You won against {0}, congrats!!!";
        private const string LooserText = "You lost to {0}, good luck next time!";

        private const int SpellShowMs = 2500;
        private const int AbilityTargetShowMs = 2000;
        private const int AttackShowMs = 1500;

        private CardsManager _cards;
        private GameManager _game;

        [SerializeField]
        private GameObject _screenOverlayCanvas = null;
        [SerializeField]
        private ModalInfo _modalInfo = null;
        [SerializeField]
        private GameObject _playedSpellCardHolder = null;
        [SerializeField]
        private LineRenderer _aimLine = null;

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
            MyCurrentSpellLogic.OnSpellReturnedToHand += CardDeselected;
            MyCurrentSpellLogic.OnSpellDroppedToTarget += SpellPlayed;
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
            MyCurrentSpellLogic.OnSpellReturnedToHand -= CardDeselected;
            MyCurrentSpellLogic.OnSpellDroppedToTarget -= SpellPlayed;
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
            CardPlayPayload payload = JsonConvert.DeserializeObject<CardPlayPayload>(resp.Payload.ToString());
            if (!payload.Approved)  // If not approved means some cheat attempt or net bug. How resolve?
            {
                Debug.LogError(resp.Error);
            }
        }   
        
        public void EnemyCardPlayed(EnemyCardPlayPayload data)
        {
            _enemyHand.PlayCardFromHand();

            Card card = _cards.GetCardById(data.CardId);
            BaseCardScript cardScript = ConvertTakenCardToScript(card, data.DeckCardId);

            _enemyResources.text = (EnemyResources - card.Cost).ToString();

            if (!(cardScript is SpellCardScript))
            {
                _enemyBoard.AddEnemyCardOnBoard(cardScript);
            }
            else
            {
                ShowSpell((SpellCardScript) cardScript);
            }
        }

        public void MyAbilityUsed(WSResponse resp)
        {
            AbilityUsePayload payload = JsonConvert.DeserializeObject<AbilityUsePayload>(resp.Payload.ToString());
            if (!payload.Approved)  // If not approved means some cheat attempt or net bug. How resolve?
            {
                Debug.LogError($"{resp.Error} with card deck id {payload.DeckCardId}!");
            }
        }

        public void EnemyAbilityUsed(EnemyAbilityUsePayload data)
        {
            Card abilityCard = _cards.GetCardById(data.CardId);

            if (data.TargetDeckCardId != -1)
            {
                if (data.TargetDeckCardId == 0)  // If base
                {
                    BaseBuildingScript baseBuilding = null;
                    if (data.Target.HasFlag(AbilityTargetType.Ally))
                        baseBuilding = _myBase;
                    else if (data.Target.HasFlag(AbilityTargetType.Enemy))
                        baseBuilding = _enemyBase;

                    if (abilityCard.Ability.Type == AbilityType.Damage)
                    {
                        baseBuilding.ShiftStats(-abilityCard.Ability.Health);
                    }
                    else if (abilityCard.Ability.Type == AbilityType.Buff)
                    {
                        baseBuilding.ShiftStats(abilityCard.Ability.Health);
                    }

                    baseBuilding.TemporaryTargetHighlight(AbilityTargetShowMs);
                }
                else   // If card
                {
                    EntityCardScript card = null;
                    if (data.Target.HasFlag(AbilityTargetType.Ally))
                    {
                        card = FindCardOnBoard(_myBoard, data.TargetDeckCardId);
                    }
                    else if (data.Target.HasFlag(AbilityTargetType.Enemy))
                    {
                        card = FindCardOnBoard(_enemyBoard, data.TargetDeckCardId);
                    }

                    if (abilityCard.Ability.Type == AbilityType.Damage)
                    {
                        card.ShiftStats(-abilityCard.Ability.Health, -abilityCard.Ability.Attack);
                    }
                    else if (abilityCard.Ability.Type == AbilityType.Buff)
                    {
                        card.ShiftStats(abilityCard.Ability.Health, abilityCard.Ability.Attack);
                    }

                    ((BaseBoardLogic) card.Logic).TemporaryTargetHighlight(AbilityTargetShowMs);
                }
            }
            else
            {
                if (data.Target.HasFlag(AbilityTargetType.Ally))  // If friend haas been damaged
                {
                    foreach (var myBoardCard in _myBoard.OnBoardCards)
                    {
                        EntityCardScript entityCard = null;
                        if (abilityCard.Ability.TargetType.HasFlag(CardType.Unit))  // If unit been damaged
                        {
                            entityCard = myBoardCard.GetComponentInChildren<UnitCardScript>();
                        }

                        if (entityCard == null && abilityCard.Ability.TargetType.HasFlag(CardType.Building))   // If building been damaged
                        {
                            entityCard = myBoardCard.GetComponentInChildren<BuildingCardScript>();
                        }

                        if (entityCard != null)
                        {
                            if (abilityCard.Ability.Type == AbilityType.Damage)
                            {
                                entityCard.ShiftStats(-abilityCard.Ability.Health, -abilityCard.Ability.Attack);
                            }
                            else if (abilityCard.Ability.Type == AbilityType.Buff)
                            {
                                entityCard.ShiftStats(abilityCard.Ability.Health, abilityCard.Ability.Attack);
                            }

                            ((BaseBoardLogic) entityCard.Logic).TemporaryTargetHighlight(AbilityTargetShowMs);
                        }
                    }

                    if (abilityCard.Ability.TargetType.HasFlag(CardType.Base))   // If base been damaged
                    {
                        if (abilityCard.Ability.Type == AbilityType.Damage)
                        {
                            _myBase.ShiftStats(-abilityCard.Ability.Health);
                        }
                        else if (abilityCard.Ability.Type == AbilityType.Buff)
                        {
                            _myBase.ShiftStats(abilityCard.Ability.Health);
                        }

                        _myBase.TemporaryTargetHighlight(AbilityTargetShowMs);
                    }
                }

                if (data.Target.HasFlag(AbilityTargetType.Enemy))
                {
                    foreach (var enemyBoardCard in _enemyBoard.OnBoardCards)
                    {
                        EntityCardScript entityCard = null;
                        if (abilityCard.Ability.TargetType.HasFlag(CardType.Unit))  
                        {
                            entityCard = enemyBoardCard.GetComponentInChildren<UnitCardScript>();
                        }

                        if (entityCard == null && abilityCard.Ability.TargetType.HasFlag(CardType.Building)) 
                        {
                            entityCard = enemyBoardCard.GetComponentInChildren<BuildingCardScript>();
                        }

                        if (entityCard != null)
                        {
                            if (abilityCard.Ability.Type == AbilityType.Damage)
                            {
                                entityCard.ShiftStats(-abilityCard.Ability.Health, -abilityCard.Ability.Attack);
                            }
                            else if (abilityCard.Ability.Type == AbilityType.Buff)
                            {
                                entityCard.ShiftStats(abilityCard.Ability.Health, abilityCard.Ability.Attack);
                            }

                            ((BaseBoardLogic) entityCard.Logic).TemporaryTargetHighlight(AbilityTargetShowMs);
                        }
                    }

                    if (abilityCard.Ability.TargetType.HasFlag(CardType.Base))
                    {
                        if (abilityCard.Ability.Type == AbilityType.Damage)
                        {
                            _enemyBase.ShiftStats(-abilityCard.Ability.Health);
                        }
                        else if (abilityCard.Ability.Type == AbilityType.Buff)
                        {
                            _enemyBase.ShiftStats(abilityCard.Ability.Health);
                        }

                        _enemyBase.TemporaryTargetHighlight(AbilityTargetShowMs);
                    }
                }
            }

            RemoveAllDeadMinions();
            CheckBasesStatus();
        }

        public void MyAttacked(WSResponse resp)
        {
            AttackPayload payload = JsonConvert.DeserializeObject<AttackPayload>(resp.Payload.ToString());
            if (!payload.Approved)  // If not approved means some cheat attempt or net bug. How resolve?
            {
                Debug.LogError($"{resp.Error} with card deck id {payload.DeckCardId}!");
            }
        }

        public void EnemyAttacked(EnemyAttackPayload data)
        {
            EntityCardScript attackCard = FindCardOnBoard(_enemyBoard, data.DeckCardId);

            if (data.TargetDeckCardId == 0)  // If base
            {
                _myBase.Damage(attackCard.Attack);

                ((BaseBoardLogic) attackCard.Logic).TemporaryAttackerHighlight(AttackShowMs);
                _myBase.TemporaryTargetHighlight(AttackShowMs);
            }
            else   // If card
            {
                EntityCardScript targetCard = FindCardOnBoard(_myBoard, data.TargetDeckCardId);

                targetCard.Damage(attackCard.Attack);
                attackCard.Damage(targetCard.Attack);

                ((BaseBoardLogic) attackCard.Logic).TemporaryAttackerHighlight(AttackShowMs);
                ((BaseBoardLogic) targetCard.Logic).TemporaryTargetHighlight(AttackShowMs);
            }

            RemoveAllDeadMinions();
            CheckBasesStatus();
        }

        public void GameFinished(GameFinishPayload data)
        {
            string text = "";
            if (data.IsWinner)
            {
                text = string.Format(WinnerText, _enemyName.text);
            }
            else
            {
                text = string.Format(LooserText, _enemyName.text);
            }

            _modalInfo.SetInfoDialog(text, OnExitClick);
        }

        public async void OnEndTurnClick()
        {
            await _game.SendEndTurnMessage();
        }

        public async void OnExitClick()
        {
            SceneManager.LoadSceneAsync("RicoClient/Scenes/MenuScene");

            await _game.CloseSocket();
        }

        private void OnWebsocketMessage(WSResponse msg)
        {
            try
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
                    case ResponseCommandType.UseAbilityResponse:
                        MyAbilityUsed(msg);
                        break;
                    case ResponseCommandType.EnemyUseAbilityResponse:
                        EnemyAbilityUsePayload enemyAbilityUseData = JsonConvert.DeserializeObject<EnemyAbilityUsePayload>(msg.Payload.ToString());
                        EnemyAbilityUsed(enemyAbilityUseData);
                        break;
                    case ResponseCommandType.AttackResponse:
                        MyAttacked(msg);
                        break;
                    case ResponseCommandType.EnemyAttackResponse:
                        EnemyAttackPayload enemyAttackData = JsonConvert.DeserializeObject<EnemyAttackPayload>(msg.Payload.ToString());
                        EnemyAttacked(enemyAttackData);
                        break;
                    case ResponseCommandType.Finished:
                        GameFinishPayload finishData = JsonConvert.DeserializeObject<GameFinishPayload>(msg.Payload.ToString());
                        GameFinished(finishData);
                        break;
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        private void OnWebsocketError(string error)
        {
            Debug.LogError($"An error has occurred during game: {error}!");
        }

        private async void ShowSpell(SpellCardScript cardScript)
        {
            var initialSize = _playedSpellCardHolder.GetComponent<RectTransform>().sizeDelta;
            RectTransform cardTransform = cardScript.GetComponent<RectTransform>();
            cardTransform.SetParent(_playedSpellCardHolder.transform, false);
            cardTransform.localPosition = Vector3.zero;
            cardTransform.sizeDelta = initialSize;

            await UniTask.Delay(SpellShowMs);
            Destroy(cardScript.gameObject);
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

            if (!(card is SpellCardScript))
            {
                card.Select(_screenOverlayCanvas.transform);
                _myBoard.HighlightAreaWith(card);
            }
            else
            {
                Vector3[] corners = new Vector3[4];
                _myBase.GetComponent<RectTransform>().GetWorldCorners(corners);
                Vector3 upperBaseSide = new Vector3((corners[1].x + corners[2].x) / 2, corners[1].y, corners[1].z);
                _aimLine.SetPositions(new Vector3[] { upperBaseSide, upperBaseSide });

                card.ActiveSpell(_screenOverlayCanvas.transform, _aimLine);

                ActivateWarcryAbility(card);
            }

            _myHand.BlockHand();
        }

        private void CardDeselected(BaseCardScript card)
        {
            if (!(card is SpellCardScript))
            {
                _myBoard.RemoveHighlight();
            }
            else
            {
                ClearAllHighlights();
            }
            
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

        private void SpellPlayed(GameObject inHandHolder, BaseCardScript card)
        {
            _myResources.text = (MyResources - card.Cost).ToString();

            ClearAllHighlights();
            Destroy(inHandHolder);
            Destroy(card.gameObject);
            _myHand.RecalculateSpacing(1);
            _myHand.EnableHand();
        }

        private EntityCardScript FindCardOnBoard(BoardScript board, int deckCardId)
        {
            foreach (var boardCard in board.OnBoardCards)
            {
                EntityCardScript card = boardCard.GetComponentInChildren<EntityCardScript>();
                if (card.DeckCardId == deckCardId)
                    return card;
            }

            return null;
        }

        private async void ActivateWarcryAbility(BaseCardScript cardScript)
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
                    EntityCardScript entityCard = null;
                    if (card.Ability.TargetType.HasFlag(CardType.Unit))
                    {
                        entityCard = myBoardCard.GetComponentInChildren<UnitCardScript>();
                    }
                    
                    if (entityCard == null && card.Ability.TargetType.HasFlag(CardType.Building))
                    {
                        entityCard = myBoardCard.GetComponentInChildren<BuildingCardScript>();
                    }

                    if (entityCard != null)
                    {
                        BaseBoardLogic cardLogic = (BaseBoardLogic) entityCard.Logic;
                        cardLogic.HighlightCard();
                        highlitedCards++;
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
                    EntityCardScript entityCard = null;
                    if (card.Ability.TargetType.HasFlag(CardType.Unit))
                    {
                        entityCard = enemyBoardCard.GetComponentInChildren<UnitCardScript>();
                    }

                    if (entityCard == null && card.Ability.TargetType.HasFlag(CardType.Building))
                    {
                        entityCard = enemyBoardCard.GetComponentInChildren<BuildingCardScript>();
                    }

                    if (entityCard != null)
                    {
                        BaseBoardLogic cardLogic = (BaseBoardLogic) entityCard.Logic;
                        cardLogic.HighlightCard();
                        highlitedCards++;
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
                if (!(cardScript is SpellCardScript))
                {
                    ((MyBoardCardLogic) cardScript.Logic).ActivateDirectedAbility();
                }

                _currentAbilityCard = cardScript;
            }
            else
            {
                // If not a spell and no targets - ignore ability
                if (!(cardScript is SpellCardScript))
                {
                    await _game.SendUsedAbilityMessage(cardScript.DeckCardId, -1, AbilityTargetType.None);
                }
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
                MyBoardCardLogic logic = _currentAbilityCard.Logic as MyBoardCardLogic; 
                if (logic != null)
                    logic.SetAimTarget(targetDownSide);
                else
                    ((MyCurrentSpellLogic) _currentAbilityCard.Logic).SetAimTarget(targetDownSide);
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
                MyBoardCardLogic logic = _currentAbilityCard.Logic as MyBoardCardLogic;
                if (logic != null)
                    logic.RemoveAimTarget();
                else
                    ((MyCurrentSpellLogic) _currentAbilityCard.Logic).RemoveAimTarget();
            }
        }

        private async void CardBeenChoosedForAction(BaseCardScript card)
        {
            if (_currentAttackingCard != null)
            {EntityCardScript entity = (EntityCardScript) card;
                entity.Damage(_currentAttackingCard.Attack);
                _currentAttackingCard.Damage(entity.Attack);

                MyBoardCardLogic attackingLogic = (MyBoardCardLogic) _currentAttackingCard.Logic;
                attackingLogic.RemoveAimTarget();
                attackingLogic.CanAttack = false;

                await _game.SendAttackedMessage(_currentAttackingCard.DeckCardId, card.DeckCardId);
            }
            else if (_currentAbilityCard != null)
            {
                Card currentCard = _cards.GetCardById(_currentAbilityCard.CardId);

                // Remove every highlight and aim line
                MyBoardCardLogic abilityLogic = _currentAbilityCard.Logic as MyBoardCardLogic;
                if (abilityLogic != null)
                {
                    abilityLogic.RemoveAimTarget();
                    abilityLogic.DeactivateDirectedAbility();
                }
                else
                {
                    ((MyCurrentSpellLogic) _currentAbilityCard.Logic).IsApllied = true;
                    await _game.SendPlacedCardMessage(_currentAbilityCard.DeckCardId);
                }

                if (currentCard.Ability.TargetCount == 1)
                {
                    EntityCardScript entity = (EntityCardScript) card;

                    AbilityTargetType targetType = entity.Logic is MyBoardCardLogic ? AbilityTargetType.Ally : AbilityTargetType.Enemy;
                    await _game.SendUsedAbilityMessage(_currentAbilityCard.DeckCardId, card.DeckCardId, targetType);

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
                    await _game.SendUsedAbilityMessage(_currentAbilityCard.DeckCardId, -1, currentCard.Ability.Target);

                    ApplyAbilityOnEverybody(currentCard);
                }

                ClearAllHighlights();
            }

            RemoveAllDeadMinions();
            CheckBasesStatus();
        }

        private async void BaseBeenChoosedForAction(BaseBuildingScript baseBuilding)
        {
            if (_currentAttackingCard != null)
            {
                baseBuilding.Damage(_currentAttackingCard.Attack);

                MyBoardCardLogic attackingLogic = (MyBoardCardLogic) _currentAttackingCard.Logic;
                attackingLogic.RemoveAimTarget();
                attackingLogic.CanAttack = false;

                await _game.SendAttackedMessage(_currentAttackingCard.DeckCardId, 0);
            }
            else if (_currentAbilityCard != null)
            {
                Card currentCard = _cards.GetCardById(_currentAbilityCard.CardId);

                // Remove every highlight and aim line
                MyBoardCardLogic abilityLogic = _currentAbilityCard.Logic as MyBoardCardLogic;
                if (abilityLogic != null)
                {
                    abilityLogic.RemoveAimTarget();
                    abilityLogic.DeactivateDirectedAbility();
                }
                else
                {
                    ((MyCurrentSpellLogic) _currentAbilityCard.Logic).IsApllied = true;
                    await _game.SendPlacedCardMessage(_currentAbilityCard.DeckCardId);
                }

                if (currentCard.Ability.TargetCount == 1)
                {
                    AbilityTargetType targetType = baseBuilding == _myBase ? AbilityTargetType.Ally : AbilityTargetType.Enemy;
                    await _game.SendUsedAbilityMessage(_currentAbilityCard.DeckCardId, 0, targetType);

                    if (currentCard.Ability.Type == AbilityType.Damage)
                    {
                        baseBuilding.ShiftStats(-currentCard.Ability.Health);
                    }
                    else if (currentCard.Ability.Type == AbilityType.Buff)
                    {
                        baseBuilding.ShiftStats(currentCard.Ability.Health);
                    }
                }
                else if (currentCard.Ability.TargetCount == -1)
                {
                    await _game.SendUsedAbilityMessage(_currentAbilityCard.DeckCardId, -1, currentCard.Ability.Target);

                    ApplyAbilityOnEverybody(currentCard);
                }

                ClearAllHighlights();

                RemoveAllDeadMinions();
                CheckBasesStatus();
            }
        }

        private void ClearAllHighlights()
        {
            foreach (var myOnBoardCard in _myBoard.OnBoardCards)
            {
                BaseBoardLogic cardLogic = (BaseBoardLogic) myOnBoardCard.GetComponentInChildren<BaseCardScript>().Logic;
                cardLogic.UnhighlightCard();
            }
            foreach (var enemyOnBoardCard in _enemyBoard.OnBoardCards)
            {
                BaseBoardLogic cardLogic = (BaseBoardLogic) enemyOnBoardCard.GetComponentInChildren<BaseCardScript>().Logic;
                cardLogic.UnhighlightCard();
            }
            _myBase.Unhighlight();
            _enemyBase.Unhighlight();
        }

        private void RemoveAllDeadMinions()
        {
            for (int i = 0; i < _myBoard.OnBoardCards.Count; i++)
            {
                var myOnBoardCard = _myBoard.OnBoardCards[i];
                EntityCardScript minionCard = myOnBoardCard.GetComponentInChildren<EntityCardScript>();
                if (minionCard.Health <= 0)
                {
                    _myBoard.OnBoardCards.RemoveAt(i);
                    Destroy(myOnBoardCard);
                    i--;
                }
            }
            for (int i = 0; i < _enemyBoard.OnBoardCards.Count; i++)
            {
                var enemyOnBoardCard = _enemyBoard.OnBoardCards[i];
                EntityCardScript minionCard = enemyOnBoardCard.GetComponentInChildren<EntityCardScript>();
                if (minionCard.Health <= 0)
                {
                    _enemyBoard.OnBoardCards.RemoveAt(i);
                    Destroy(enemyOnBoardCard);
                    i--;
                }
            }
        }

        private void CheckBasesStatus()
        {
            bool isMyBaseDead = _myBase.Health <= 0;
            bool isEnemyBaseDead = _enemyBase.Health <= 0;

            // ToDo
        }

        private void ApplyAbilityOnEverybody(Card currentCard)
        {
            if (currentCard.Ability.Target.HasFlag(AbilityTargetType.Ally))  // If friend can be damaged
            {
                foreach (var myBoardCard in _myBoard.OnBoardCards)
                {
                    EntityCardScript entityCard = null;
                    if (currentCard.Ability.TargetType.HasFlag(CardType.Unit))  // If unit can be damaged
                    {
                        entityCard = myBoardCard.GetComponentInChildren<UnitCardScript>();
                    }

                    if (entityCard == null && currentCard.Ability.TargetType.HasFlag(CardType.Building))   // If building can be damaged
                    {
                        entityCard = myBoardCard.GetComponentInChildren<BuildingCardScript>();
                    }

                    if (entityCard != null)
                    {
                        if (currentCard.Ability.Type == AbilityType.Damage)
                        {
                            entityCard.ShiftStats(-currentCard.Ability.Health, -currentCard.Ability.Attack);
                        }
                        else if (currentCard.Ability.Type == AbilityType.Buff)
                        {
                            entityCard.ShiftStats(currentCard.Ability.Health, currentCard.Ability.Attack);
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
                    EntityCardScript entityCard = null;
                    if (currentCard.Ability.TargetType.HasFlag(CardType.Unit))  // If unit can be damaged
                    {
                        entityCard = enemyBoardCard.GetComponentInChildren<UnitCardScript>();
                    }

                    if (entityCard == null && currentCard.Ability.TargetType.HasFlag(CardType.Building))   // If building can be damaged
                    {
                        entityCard = enemyBoardCard.GetComponentInChildren<BuildingCardScript>();
                    }

                    if (entityCard != null)
                    {
                        if (currentCard.Ability.Type == AbilityType.Damage)
                        {
                            entityCard.ShiftStats(-currentCard.Ability.Health, -currentCard.Ability.Attack);
                        }
                        else if (currentCard.Ability.Type == AbilityType.Buff)
                        {
                            entityCard.ShiftStats(currentCard.Ability.Health, currentCard.Ability.Attack);
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
