using RicoClient.Scripts.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using Zenject;

namespace RicoClient.Scripts.Menu.Collection.Panels
{
    public class DeckPanelScript : MonoBehaviour
    {
        private CardsManager _cards;

        [SerializeField]
        private DeckCardScript _deckCardPrefab = null;
        [SerializeField]
        private GameObject _deckContent = null;
        [SerializeField]
        private TMP_InputField _deckName = null;

        public uint DeckId { get; private set; }

        private List<DeckCardScript> _deckCards;

        [Inject]
        public void Initialize(CardsManager cards)
        {
            _cards = cards;
        }

        protected void OnDisable()
        {
            _deckCards.Clear();
            // dont forget to clean everything
        }

        public void OnDeckDeleteClicked()
        {
            // Open modal and ask and then send req to the server and return to decks
        }

        public void SetDeck(int id, string deckname, List<int?> cards)
        {
            // Get cards from db and save them
        }

        public void AddCardInDeck()
        {
            // Add to list
        }
    }
}
