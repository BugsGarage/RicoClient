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
        [SerializeField]
        private TMP_Text _cardName = null;
        [SerializeField]
        private TMP_Text _cardAmount = null;
        [SerializeField]
        private TMP_Text _cardResourceCost = null;

        public int CardId { get; private set; }

        public void SetDeckCard(int id, string name, int amount, int cost)
        {
            CardId = id;

            _cardName.text = name;
            _cardAmount.text = amount.ToString();
            _cardResourceCost.text = cost.ToString();
        }

        public void OnDeckCardClick()
        {
            // del from deck
        }
    }
}
