using RicoClient.Scripts.Decks;
using RicoClient.Scripts.Menu.Collection;
using RicoClient.Scripts.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace RicoClient.Scripts.Menu.Collection.Panels
{
    public class DecksPanelScript : MonoBehaviour
    {
        private DeckManager _deckManager;

        [SerializeField]
        private DeckListPanelScript _deckList = null;
        [SerializeField]
        private DeckPanelScript _deck = null;

        [Inject]
        public void Initialize(DeckManager deck)
        {
            _deckManager = deck;
        }

        protected void OnEnable()
        {
            OpenDeckListPanel();

            _deckList.OnDeckOpen += OnDeckOpened;
            _deck.OnDeckDelete += OnDeckDeleted;
        }

        protected void OnDisable()
        {
            _deckList.OnDeckOpen -= OnDeckOpened;
            _deck.OnDeckDelete -= OnDeckDeleted;

            _deckList.gameObject.SetActive(false);
            _deck.gameObject.SetActive(false);
        }

        public void OnCreateDeck()
        {
            OpenDeckPanel();
        }

        public async void OnConfirmDeckEdit()
        {
            // Send changes to the server

            OpenDeckListPanel();
        }

        public void OnCancelDeckEdit()
        {
            OpenDeckListPanel();
        }

        private void OpenDeckPanel()
        {
            _deckList.gameObject.SetActive(false);
            _deck.gameObject.SetActive(true);
        }

        private void OpenDeckListPanel()
        {
            _deck.gameObject.SetActive(false);
            _deckList.gameObject.SetActive(true);
        }

        private async void OnDeckOpened(DeckScript deckHeader)
        {
            var deck = await _deckManager.DeckById(deckHeader.DeckId);
            _deck.SetDeck(deck);

            OpenDeckPanel();
        }

        private async void OnDeckDeleted()
        {
            // Send del req to the server

            OpenDeckListPanel();
        }
    }
}
