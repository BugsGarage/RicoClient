using RicoClient.Scripts.Cards;
using RicoClient.Scripts.Cards.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace RicoClient.Scripts.Menu.Collection
{
    public class CardHolderScript : MonoBehaviour
    {
        public event Action<BaseCardScript> OnCardRightClick;

        [SerializeField]
        private UnitCardScript _unitCard = null;
        [SerializeField]
        private BuildingCardScript _buildingCard = null;
        [SerializeField]
        private SpellCardScript _spellCard = null;
        [SerializeField]
        private TMP_Text _cardAmount = null;

        private BaseCardScript _activeCard;

        public void OnEnable()
        {
            _unitCard.OnCardRightClick += OnCardRightClicked;
            _buildingCard.OnCardRightClick += OnCardRightClicked;
            _spellCard.OnCardRightClick += OnCardRightClicked;
        }

        public void OnDisable()
        {
            _unitCard.OnCardRightClick -= OnCardRightClicked;
            _buildingCard.OnCardRightClick -= OnCardRightClicked;
            _spellCard.OnCardRightClick -= OnCardRightClicked;
        }

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
            
            if (_activeCard != null)
                _activeCard.SetActive(active);
        }

        public void PlaceCard(Card card, int amount)
        {
            switch (card.Type)
            {
                case CardType.Unit:
                    _unitCard.FillCard(card);
                    _activeCard = _unitCard;
                    break;
                case CardType.Building:
                    _buildingCard.FillCard(card);
                    _activeCard = _buildingCard;
                    break;
                case CardType.Spell:
                    _spellCard.FillCard(card);
                    _activeCard = _spellCard;
                    break;
                default:
                    throw new NotSupportedException($"Some not supported cards type ({card.Type}) arrived!");
            }

            _cardAmount.text = amount.ToString();

            SetActive(true);
        }

        private void OnCardRightClicked(BaseCardScript card)
        {
            OnCardRightClick?.Invoke(card);
        }
    }
}
