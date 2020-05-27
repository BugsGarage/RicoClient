using RicoClient.Scripts.Cards;
using RicoClient.Scripts.Cards.Entities;
using System;
using UnityEngine;

namespace RicoClient.Scripts.Menu.Shop
{
    public class BoughtCardModalScript : MonoBehaviour
    {
        [SerializeField]
        private UnitCardScript _unitCard = null;
        [SerializeField]
        private BuildingCardScript _buildingCard = null;
        [SerializeField]
        private SpellCardScript _spellCard = null;

        private BaseCardScript _activeCard;

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);

            if (_activeCard != null)
                _activeCard.SetActive(active);
        }

        public void PlaceCard(Card card)
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

            SetActive(true);
        }
    }
}
