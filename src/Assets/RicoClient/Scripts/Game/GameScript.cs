using RicoClient.Scripts.Cards;
using RicoClient.Scripts.Cards.Entities;
using RicoClient.Scripts.Decks;
using RicoClient.Scripts.Exceptions;
using RicoClient.Scripts.Game.CardLogic.CurrentLogic;
using RicoClient.Scripts.Game.CardLogic.HandLogic;
using RicoClient.Scripts.Game.InGameDeck;
using RicoClient.Scripts.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace RicoClient.Scripts.Game
{
    public enum GameState
    {
        Start,
        MyTurnStart,
        MyBattle,
        MyTurnEnd,
        EnemyTurnStart,
        EnemyBattle,
        EnemyTurnEnd,
        Finished
    }

    public class GameScript : MonoBehaviour
    {
        private NetworkManager _network;
        private CardsManager _cards;
        private DeckManager _deck;

        [SerializeField]
        private GameObject _screenOverlayCanvas = null;

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

        public GameState State { get; private set; }

        // temp
        private Deck PlayerDeck;
        private int PlayerDeckInitSize;
        private int EnemyDeckInitSize;
        private int _collectedTurnStartCardsCount = 1;
        private int _gameStartCardsCount = 3;

        [Inject]
        public void Initialize(NetworkManager network, CardsManager cards, DeckManager deck)
        {
            _network = network;
            _cards = cards;
            _deck = deck;

            State = GameState.Finished;

            MyHandCardLogic.OnCardSelected += CardSelected;
            MyCurrentCardLogic.OnCardReturnedToHand += CardDeselected;
            MyCurrentCardLogic.OnCardDroppedToBoard += CardDropped;

            // Also temp
            _cards.UpdateLocalCards();
        }

        protected async void Start()
        {
            if (State != GameState.Finished)
                throw new GameCoreException("Not finished previous game!");

            // Not sure how to start so let it be that way for now
            PlayerDeck = await _deck.DeckById(4);
            PlayerDeckInitSize = PlayerDeck.CardsCount;
            EnemyDeckInitSize = 10;

            State = GameState.Start;

            _myDeck.SetDeck(PlayerDeckInitSize);
            _enemyDeck.SetDeck(EnemyDeckInitSize);

            List<Card> gameStartCards = new List<Card>(_gameStartCardsCount);
            for (int i = 0; i < _gameStartCardsCount; i++)
            {
                // This is temp too
                gameStartCards.Add(GetRandomCardFromDeckMock());
            }

            foreach (var startCard in gameStartCards)
            {
                _myDeck.TakeCard();
                var convertedCard = ConvertCardToScript(startCard);
                _myHand.AddRevealedCardInHand(convertedCard);
            }

            int collectedCardsByEnemy = _gameStartCardsCount;
            for (int i = 0; i < collectedCardsByEnemy; i++)
            {
                _enemyDeck.TakeCard();
                var hiddenCard = Instantiate(_backCard);
                _enemyHand.AddHiddenCardInHand(hiddenCard);
            }

            MyTurnStart();
        }

        protected void OnDestroy()
        {
            MyHandCardLogic.OnCardSelected -= CardSelected;
            MyCurrentCardLogic.OnCardReturnedToHand -= CardDeselected;
            MyCurrentCardLogic.OnCardDroppedToBoard -= CardDropped;
        }

        public void MyTurnStart()
        {
            List<Card> turnCards = new List<Card>(_collectedTurnStartCardsCount);
            for (int i = 0; i < _collectedTurnStartCardsCount; i++)
            {
                // This is temp too
                turnCards.Add(GetRandomCardFromDeckMock());
            }

            foreach (var turnCard in turnCards)
            {
                _myDeck.TakeCard();
                var convertedCard = ConvertCardToScript(turnCard);
                _myHand.AddRevealedCardInHand(convertedCard);
            }

            BattlePhase();
        }

        public void BattlePhase()
        {
            
        }

        private void CardSelected(BaseCardScript card)
        {
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

        private void CardDropped(GameObject inHandHolder)
        {
            _myBoard.RemoveHighlight();
            Destroy(inHandHolder);
            _myHand.EnableHand();
        }

        private BaseCardScript ConvertCardToScript(Card card)
        {
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

            res.FillCard(card);
            return res;
        }

        // Temp mock
        private Card GetRandomCardFromDeckMock()
        {
            int randomCardI = UnityEngine.Random.Range(0, PlayerDeck.DeckCards.Count);
            return _cards.GetCardById(PlayerDeck.DeckCards.Keys.ElementAt(randomCardI));
        }
    }
}
