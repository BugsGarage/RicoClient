using RicoClient.Scripts.Cards;
using RicoClient.Scripts.Decks;
using RicoClient.Scripts.Menu.Modals;
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
        public event Action OnDeckDelete;

        private CardsManager _cards;

        [SerializeField]
        private ModalWarn _warnModal = null;

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
            foreach (var deckCard in _deckCards)
            {
                deckCard.OnCardDelete -= CardDeletion;
                Destroy(deckCard.gameObject);
            }

            _deckCards.Clear();
        }

        public void OnDeckDeleteClicked()
        {
            _warnModal.SetWarnDialog("Are you sure you want to delete this deck?", new Action(() => {
                OnDeckDelete?.Invoke();
            }));
        }

        public void SetDeck(Deck deck)
        {
            DeckId = deck.DeckId;
            _deckName.text = deck.DeckName;
            _deckCards = new List<DeckCardScript>(deck.DeckCards.Count);

            foreach (var deckCard in deck.DeckCards)
            {
                for (int i = 0; i < deckCard.Value; i++)
                    AddCardInDeck(deckCard.Key);
            }
        }

        public void AddCardInDeck(int cardId)
        {
            var card = _cards.GetCardById(cardId);

            int cardIndex = FindCardInDeck(cardId);
            if (cardIndex != -1)
            {
                _deckCards[cardIndex].IncreaseDeckCardAmount();
            }
            else
            {
                var deckCard = Instantiate(_deckCardPrefab.gameObject, _deckContent.transform).GetComponent<DeckCardScript>();
                deckCard.SetDeckCard(cardId, card.Name, card.Cost);
                deckCard.OnCardDelete += CardDeletion;

                _deckCards.Add(deckCard);
            }
        }

        private void CardDeletion(DeckCardScript deckCard)
        {
            for (int i = 0; i < _deckCards.Count; i++)
            {
                if (deckCard.CardId == _deckCards[i].CardId)
                {
                    _deckCards.RemoveAt(i);
                    break;
                }
            }

            Destroy(deckCard.gameObject);
        }

        private int FindCardInDeck(int cardId)
        {
            for (int i = 0; i < _deckCards.Count; i++)
                if (cardId == _deckCards[i].CardId)
                    return i;

            return -1;
        }
    }
}
