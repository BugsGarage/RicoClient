using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace RicoClient.Scripts.Menu.Collection
{
    public class DeckCardScript : MonoBehaviour
    {
        public event Action<DeckCardScript> OnCardDelete;

        [SerializeField]
        private TMP_Text _cardName = null;
        [SerializeField]
        private TMP_Text _cardAmount = null;
        [SerializeField]
        private TMP_Text _cardResourceCost = null;

        public int CardId { get; private set; }
        public int Amount { get; private set; }

        public void SetDeckCard(int id, string name, int cost)
        {
            CardId = id;
            Amount = 1;

            _cardName.text = name;
            _cardAmount.text = Amount.ToString();
            _cardResourceCost.text = cost.ToString();
        }

        public void IncreaseDeckCardAmount()
        {
            Amount++;
            _cardAmount.text = Amount.ToString();
        }

        public void OnDeckCardClick()
        {
            Amount--;

            if (Amount > 0)
                _cardAmount.text = Amount.ToString();
            else
                OnCardDelete?.Invoke(this);
        }
    }
}
