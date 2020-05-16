using RicoClient.Scripts.Cards;
using RicoClient.Scripts.Cards.Entities;
using RicoClient.Scripts.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace RicoClient.Scripts.Menu.Collection
{
    public class CollectionMenuScript : BaseMenuScript
    {
        private CardsManager _cards;

        public BaseMenuScript ReturnMenu { get; set; }

        [SerializeField]
        private CardHolderScript[] _cardHolders = null;
        [SerializeField]
        private Button _leftArrowButton = null;
        [SerializeField]
        private Button _rightArrowButton = null;
        [SerializeField]
        private TMP_Text _ownedCardsFilterButtonText = null;

        [SerializeField]
        private ModalCardScript _modalCard = null;

        private bool _isOwnedCards;

        private int _allCardsCount;
        private int _currPageNum;
        private int _maxPageNum;

        private List<int> _playerCards; 

        [Inject]
        public void Initialize(CardsManager cards)
        {
            _cards = cards;

            _isOwnedCards = false;
            _allCardsCount = 0;
            _currPageNum = 0;
            _maxPageNum = 0;
        }

        protected void OnEnable()
        {
            foreach (var cardHolder in _cardHolders)
            {
                cardHolder.OnCardClick += ModalCardShow;
            }

            _playerCards = UserManager.PlayerCards.Keys.ToList();
            _currPageNum = 0;
            _isOwnedCards = false;

            OnOwnedCardsFilterClick();

            CurrPageUpdate();
        }

        protected void OnDisable()
        {
            foreach (var cardHolder in _cardHolders)
            {
                cardHolder.OnCardClick -= ModalCardShow;
                cardHolder.SetActive(false);
            }
        }

        public void OnReturnClick()
        {
            ReturnMenu.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }

        public void OnOwnedCardsFilterClick()
        {
            _isOwnedCards = !_isOwnedCards;

            if (_isOwnedCards)
            {
                _allCardsCount = _playerCards.Count;
                _ownedCardsFilterButtonText.text = "All cards";
            }
            else
            {
                _allCardsCount = _cards.AllCards.Count;
                _ownedCardsFilterButtonText.text = "My cards";
            }

            _maxPageNum = _allCardsCount / _cardHolders.Length;
            if (_currPageNum > _maxPageNum)
                _currPageNum = 0;

            CurrPageUpdate();
        }

        public void OnLeftClick()
        {
            if (_currPageNum > 0)
            {
                _currPageNum--;
                CurrPageUpdate();
            }
        }

        public void OnRightClick()
        {
            if (_currPageNum < _maxPageNum)
            {
                _currPageNum++;
                CurrPageUpdate();
            }
        }

        /// <summary>
        /// Reads another page of cards from local db and set it up
        /// </summary>
        /// <returns></returns>
        private void CurrPageUpdate()
        {
            int holdersCount = _cardHolders.Length;

            int firstCardOnPageId = _currPageNum * holdersCount + 1;
            int diff = _allCardsCount - firstCardOnPageId + 1;
            int cardsOnPage = diff > holdersCount ? holdersCount : diff;

            Card[] currPageCards;
            if (_isOwnedCards)
                currPageCards = _cards.GetCardsRangeFrom(_playerCards, firstCardOnPageId, cardsOnPage);
            else
                currPageCards = _cards.GetAllCardsRange(firstCardOnPageId, cardsOnPage);

            for (int i = 0; i < holdersCount; i++)
            {
                _cardHolders[i].SetActive(false);

                if (i < currPageCards.Length)
                {
                    if (!UserManager.PlayerCards.TryGetValue(currPageCards[i].CardId, out int amount))
                        amount = 0;

                    _cardHolders[i].PlaceCard(currPageCards[i], amount);
                }
            }

            if (_currPageNum == 0)
                _leftArrowButton.interactable = false;
            else
                _leftArrowButton.interactable = true;

            if (_currPageNum == _maxPageNum)
                _rightArrowButton.interactable = false;
            else
                _rightArrowButton.interactable = true;
        }

        private void ModalCardShow(BaseCardScript card)
        {
            int price = _cards.GetByCardId(card.CardId).GoldCost;
            _modalCard.SetModalCard(card, price);
        }
    }
}
