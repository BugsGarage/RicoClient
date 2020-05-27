using RicoClient.Scripts.Decks;
using RicoClient.Scripts.Exceptions;
using System.Collections.Generic;
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

            DeckScript.OnDeckOpen += OnDeckOpened;
            _deck.OnDeckDelete += OnDeckDeleted;
        }

        protected void OnDisable()
        {
            DeckScript.OnDeckOpen -= OnDeckOpened;
            _deck.OnDeckDelete -= OnDeckDeleted;

            _deckList.gameObject.SetActive(false);
            _deck.gameObject.SetActive(false);
        }

        public void OnCreateDeck()
        {
            _deck.SetDeck();

            OpenDeckPanel();
        }

        public async void OnConfirmDeckEdit()
        {
            Dictionary<int, int> deckCards = new Dictionary<int, int>();
            int cardsCount = 0;
            foreach (var deckCard in _deck.DeckCards)
            {
                deckCards.Add(deckCard.CardId, deckCard.Amount);
                cardsCount += deckCard.Amount;
            }

            try
            {
                if (_deck.IsNewDeck)
                    await _deckManager.AddDeck(_deck.DeckName, deckCards, cardsCount);
                else
                    await _deckManager.ConfirmDeckChange(_deck.DeckId, _deck.DeckName, deckCards, cardsCount);
            }
            catch (PlayersException e)
            {
                Debug.LogError(e.Message);
                return;
            }

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
            Deck deck;
            try
            {
                deck = await _deckManager.DeckById(deckHeader.DeckId);
            }
            catch (PlayersException e)
            {
                Debug.LogError(e.Message);
                return;
            }
            _deck.SetDeck(deck);

            OpenDeckPanel();
        }

        private async void OnDeckDeleted()
        {
            try
            {
                if (!_deck.IsNewDeck)
                    await _deckManager.DeleteDeck(_deck.DeckId);
            }
            catch (PlayersException e)
            {
                Debug.LogError(e.Message);
                return;
            }

            OpenDeckListPanel();
        }
    }
}
