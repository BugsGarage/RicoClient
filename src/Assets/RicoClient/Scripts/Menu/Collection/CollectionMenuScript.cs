using RicoClient.Scripts.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace RicoClient.Scripts.Menu.Collection
{
    public class CollectionMenuScript : MonoBehaviour
    {
        private CardsManager _cards;

        [SerializeField]
        private CardHolderScript[] _cardHolders = null;
        [SerializeField]
        private Button _leftArrowButton = null;
        [SerializeField]
        private Button _rightArrowButton = null;

        private int _allCardsCount;
        private int _currPageNum;
        private int _maxPageNum;

        [Inject]
        public void Initialize(CardsManager cards)
        {
            _cards = cards;

            _allCardsCount = 0;
            _currPageNum = 0;
            _maxPageNum = 0;
        }

        protected async void OnEnable()
        {
            _allCardsCount = await _cards.CardsCount();
            _currPageNum = 0;
            _maxPageNum = _allCardsCount / _cardHolders.Length;

            await CurrPageUpdate();
        }

        protected void OnDisable()
        {
            foreach (var cardHolder in _cardHolders)
            {
                cardHolder.SetActive(false);
            }
        }

        public async void OnLeftClick()
        {
            if (_currPageNum > 0)
            {
                _currPageNum--;
                await CurrPageUpdate();
            }
        }

        public async void OnRightClick()
        {
            if (_currPageNum < _maxPageNum)
            {
                _currPageNum++;
                await CurrPageUpdate();
            }
        }

        /// <summary>
        /// Reads another page of cards from local db and set it up
        /// </summary>
        /// <returns></returns>
        private async UniTask CurrPageUpdate()
        {
            int holdersCount = _cardHolders.Length;
            Card[] currPageCards = await _cards.GetCardsRange(_currPageNum * holdersCount + 1, holdersCount);

            for (int i = 0; i < holdersCount; i++)
            {
                _cardHolders[i].SetActive(false);
                if (i < currPageCards.Length)
                    _cardHolders[i].PlaceCard(currPageCards[i]);
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
    }
}
