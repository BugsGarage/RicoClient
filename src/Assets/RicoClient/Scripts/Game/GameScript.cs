using RicoClient.Scripts.Cards;
using RicoClient.Scripts.Cards.Entities;
using RicoClient.Scripts.Game.CardLogic;
using RicoClient.Scripts.Game.InGameDeck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace RicoClient.Scripts.Game
{
    public class GameScript : MonoBehaviour
    {
        private GameManager _game;

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

        [Inject]
        public void Initialize(GameManager game)
        {
            _game = game;

            MyHandCardLogic.OnCardSelected += CardSelected;
            MyCurrentCardLogic.CardReturnedToHand += CardDeselected;
        }

        protected async void Start()
        {
            await _game.StartGame();

            _myDeck.SetDeck(_game.PlayerDeckInitSize);
            _enemyDeck.SetDeck(_game.EnemyDeckInitSize);

            TurnStart();
        }

        protected void OnDestroy()
        {
            MyHandCardLogic.OnCardSelected -= CardSelected;
            MyCurrentCardLogic.CardReturnedToHand -= CardDeselected;
        }

        public void TurnStart()
        {
            var collectedCards = _game.TurnStart();
            foreach (var collectedCard in collectedCards)
            {
                _myDeck.TakeCard();
                var convertedCard = ConvertCardToScript(collectedCard);
                _myHand.AddRevealedCardInHand(convertedCard);
            }

            int collectedCardsByEnemy = _game.TurnStartEnemy();
            for (int i = 0; i < collectedCardsByEnemy; i++)
            {
                _enemyDeck.TakeCard();
                var hiddenCard = Instantiate(_backCard);
                _enemyHand.AddHiddenCardInHand(hiddenCard);
            }

            PreparingPhase();
        }

        public void PreparingPhase()
        {
            _game.Preparing();
        }

        private void CardSelected(BaseCardScript card)
        {
            card.Select(_screenOverlayCanvas.transform);
            _myHand.BlockHand();

            if (!(card is SpellCardScript))
            {
                _myBoard.HighlightArea();
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
    }
}
