using RicoClient.Scripts.Cards;
using RicoClient.Scripts.Cards.Entities;
using RicoClient.Scripts.Menu.Collection.Panels;
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
        public BaseMenuScript ReturnMenu { get; set; }

        [SerializeField]
        private CardsPanelScript _cardsPanel = null;
        [SerializeField]
        private TMP_Text _ownedCardsFilterButtonText = null;

        private bool _isOwnedCards;

        protected void OnEnable()
        {
            _cardsPanel.OnCollectionPanelEnable();

            _isOwnedCards = false; 
            OnOwnedCardsFilterClick();
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
                _ownedCardsFilterButtonText.text = "All cards";
            else
                _ownedCardsFilterButtonText.text = "My cards";

            _cardsPanel.OwnedCardsFilterChange(_isOwnedCards);
        }
    }
}
