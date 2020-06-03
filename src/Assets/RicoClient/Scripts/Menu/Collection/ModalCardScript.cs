using RicoClient.Scripts.Cards;
using RicoClient.Scripts.Exceptions;
using RicoClient.Scripts.Pay;
using RicoClient.Scripts.User;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace RicoClient.Scripts.Menu.Collection
{
    public class ModalCardScript : MonoBehaviour
    {
        private const string ResourceArtDescriptionsPath = "Cards/ArtDescriptions/{0}";

        public event Action OnBoughtCard;

        private ShopManager _shop;

        [SerializeField]
        private GameObject _bigCardHolder = null;
        [SerializeField]
        private TMP_Text _price = null;
        [SerializeField]
        private TMP_Text _artDescription = null;
        [SerializeField]
        private TMP_Text _playerBalance = null;
        [SerializeField]
        private Button _buyButton = null;

        private GameObject _bigCard;
        private int _cardId;

        [Inject]
        public void Initialize(ShopManager shop)
        {
            _shop = shop;
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
            _artDescription.text = Resources.Load<TextAsset>(string.Format(ResourceArtDescriptionsPath, _cardId))?.text;

            gameObject.SetActive(true);
        }

        public async void OnBuyClick()
        {
            _buyButton.enabled = false;
            try
            {
                await _shop.BuySpecificCard(_cardId, int.Parse(_price.text));
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
            finally
            {
                _buyButton.enabled = true;
            }

            OnBoughtCard?.Invoke();
            gameObject.SetActive(false);
        }
    }
}
