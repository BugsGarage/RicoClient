using RicoClient.Scripts.Cards;
using RicoClient.Scripts.Exceptions;
using RicoClient.Scripts.Pay;
using RicoClient.Scripts.User;
using TMPro;
using UnityEngine;
using Zenject;

namespace RicoClient.Scripts.Menu.Shop
{
    public class ShopMenuScript : BaseMenuScript
    {
        private ShopManager _pay;
        private CardsManager _cards;

        [SerializeField]
        private TMP_Text _playerBalance = null;
        [SerializeField]
        private TMP_Text _goldCoef = null;

        [SerializeField]
        private BoughtCardModalScript _boughtCard = null;
        [SerializeField]
        private TMP_Text _randomCardCost = null;

        [Inject]
        public void Initialize(ShopManager pay, CardsManager cards)
        {
            _pay = pay;
            _cards = cards;
        }

        protected async void OnEnable()
        {
            _playerBalance.text = UserManager.Balance.ToString();

            try
            {
                _randomCardCost.text = (await _pay.GetRandomCardCost()).ToString();
            }
            catch (ShopException e)
            {
                _randomCardCost.text = "?";
                Debug.LogError(e.Message);
                return;
            }

            try
            {
                _goldCoef.text = (await _pay.GetGoldCoeff()).ToString();
            }
            catch (PaymentException e)
            {
                _goldCoef.text = "?";
                Debug.LogError(e.Message);
                return;
            }
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
            catch (ShopException e)
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
            try
            {
                await _pay.BuyGold(10);
            }
            catch (PaymentException e)
            {
                Debug.LogError(e.Message);
                return;
            }

            _playerBalance.text = UserManager.Balance.ToString();
        }

        public async void On100GoldBuyClick()
        {
            try
            {
                await _pay.BuyGold(100);
            }
            catch (PaymentException e)
            {
                Debug.LogError(e.Message);
                return;
            }

            _playerBalance.text = UserManager.Balance.ToString();
        }

        public async void On1000GoldBuyClick()
        {
            try
            {
                await _pay.BuyGold(1000);
            }
            catch (PaymentException e)
            {
                Debug.LogError(e.Message);
                return;
            }

            _playerBalance.text = UserManager.Balance.ToString();
        }
    }
}
