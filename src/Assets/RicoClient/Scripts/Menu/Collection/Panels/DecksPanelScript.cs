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
        private UserManager _user;

        [SerializeField]
        private DeckListPanelScript _deckList = null;
        [SerializeField]
        private DeckPanelScript _deck = null;

        [Inject]
        public void Initialize(UserManager user)
        {
            _user = user;
        }

        protected void OnEnable()
        {
            _deckList.gameObject.SetActive(true);

            _deckList.OnDeckOpen += OnDeckOpened;
        }

        protected void OnDisable()
        {
            _deckList.OnDeckOpen -= OnDeckOpened;

            _deckList.gameObject.SetActive(false);
            _deck.gameObject.SetActive(false);
        }

        public void OnCreateDeck()
        {
            _deckList.gameObject.SetActive(false);
            _deck.gameObject.SetActive(true);
        }

        public void OnConfirmDeckEdit()
        {
            // Send changes to server

            _deck.gameObject.SetActive(false);
            _deckList.gameObject.SetActive(true);
        }

        public void OnCancelDeckEdit()
        {
            _deck.gameObject.SetActive(false);
            _deckList.gameObject.SetActive(true);
        }

        public void OnDeckOpened(DeckScript deck)
        {
            // Req for deck cards here

            _deckList.gameObject.SetActive(false);
            _deck.gameObject.SetActive(true);
        }
    }
}
