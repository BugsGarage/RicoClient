using RicoClient.Scripts.Cards;
using RicoClient.Scripts.Exceptions;
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

namespace RicoClient.Scripts.Menu.Collection
{
    public class ModalCardScript : MonoBehaviour
    {
        public event Action OnBoughtCard;

        private PayManager _pay;

        [SerializeField]
        private GameObject _bigCardHolder = null;
        [SerializeField]
        private TMP_Text _price = null;
        [SerializeField]
        private TMP_Text _playerBalance = null;

        private GameObject _bigCard;
        private int _cardId;

        [Inject]
        public void Initialize(PayManager pay)
        {
            _pay = pay;
        }

        public void OnEnable()
        {
            _playerBalance.text = UserManager.Balance.ToString();
        }

        public void OnDisable()
        {
            Destroy(_bigCard);
        }

        public void SetModalCard(BaseCardScript card, int price)
        {
            _bigCard = Instantiate(card.gameObject, _bigCardHolder.transform);
            _cardId = card.CardId;
            _price.text = price.ToString();

            gameObject.SetActive(true);
        }

        public async void OnBuyClick()
        {
            try
            {
                await _pay.BuySpecificCard(_cardId, int.Parse(_price.text));
            }
            catch (NotEnoughBalanceException e)
            {
                Debug.LogError(e.Message);
                return;
            }

            OnBoughtCard?.Invoke();
            gameObject.SetActive(false);
        }
    }
}
