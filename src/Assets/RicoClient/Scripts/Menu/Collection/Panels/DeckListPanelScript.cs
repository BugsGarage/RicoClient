using RicoClient.Scripts.Menu.Collection;
using RicoClient.Scripts.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RicoClient.Scripts.Menu.Collection.Panels
{
    public class DeckListPanelScript : MonoBehaviour
    {
        [SerializeField]
        private DeckScript _deckPrefab = null;
        [SerializeField]
        private GameObject _deckListContent = null;

        private List<DeckScript> _decks;

        protected void OnEnable()
        {
            _decks = new List<DeckScript>();

            UpdateDeckList();
        }

        protected void OnDisable()
        {
            CleanDeckList();
        }

        public void UpdateDeckList()
        {
            CleanDeckList();

            var decks = UserManager.DeckHeaders;
            _decks = new List<DeckScript>(decks.Count);

            foreach (var deck in decks)
            {
                var currDeck = Instantiate(_deckPrefab.gameObject, _deckListContent.transform).GetComponent<DeckScript>();
                currDeck.SetDeck(deck.DeckId, deck.DeckName);

                _decks.Add(currDeck);
            }
        }

        private void CleanDeckList()
        {
            foreach (var deck in _decks)
            {
                Destroy(deck.gameObject);
            }

            _decks.Clear();
        }
    }
}
