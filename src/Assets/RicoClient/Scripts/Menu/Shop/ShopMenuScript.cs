using RicoClient.Scripts.Cards;
using RicoClient.Scripts.Exceptions;
using RicoClient.Scripts.Menu.Collection;
using RicoClient.Scripts.Pay;
using RicoClient.Scripts.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using Zenject;

namespace RicoClient.Scripts.Menu.Shop
{
    public class ShopMenuScript : BaseMenuScript
    {
        private PayManager _pay;
        private CardsManager _cards;

        [SerializeField]
        private TMP_Text _playerBalance = null;

        [SerializeField]
        private BoughtCardModalScript _boughtCard = null;
        [SerializeField]
        private TMP_Text _randomCardCost = null;

        [Inject]
        public void Initialize(PayManager pay, CardsManager cards)
        {
            _pay = pay;
            _cards = cards;
        }

        protected async void OnEnable()
        {
            _playerBalance.text = UserManager.Balance.ToString();
            _randomCardCost.text = (await _pay.GetRandomCardCost()).ToString();
        }

        public async void OnRandomBuyClick()
        {
            int cardId;
            try
            {
                cardId = await _pay.BuyRandomCard(int.Parse(_randomCardCost.text));
            }
            catch (NotEnoughBalanceException e)
            {
                Debug.LogError(e.Message);
                return;
            }

            var card = _cards.GetCardById(cardId);

            _boughtCard.PlaceCard(card);
            _playerBalance.text = UserManager.Balance.ToString();
        }

        public async void On10GoldBuyClick()
        {
            bool res = await _pay.BuyGold(10);
            if (res)
                _playerBalance.text = UserManager.Balance.ToString();
        }

        public async void On100GoldBuyClick()
        {
            bool res = await _pay.BuyGold(100);
            if (res)
                _playerBalance.text = UserManager.Balance.ToString();
        }

        public async void On1000GoldBuyClick()
        {
            bool res = await _pay.BuyGold(1000);
            if (res)
                _playerBalance.text = UserManager.Balance.ToString();
        }
    }
}
