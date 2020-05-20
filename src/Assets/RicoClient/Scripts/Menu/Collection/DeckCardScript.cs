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

        private int _amount;

        public void SetDeckCard(int id, string name, int cost)
        {
            CardId = id;
            _amount = 1;

            _cardName.text = name;
            _cardAmount.text = _amount.ToString();
            _cardResourceCost.text = cost.ToString();
        }

        public void IncreaseDeckCardAmount()
        {
            _amount++;
            _cardAmount.text = _amount.ToString();
        }

        public void OnDeckCardClick()
        {
            _amount--;

            if (_amount > 0)
                _cardAmount.text = _amount.ToString();
            else
                OnCardDelete?.Invoke(this);
        }
    }
}
