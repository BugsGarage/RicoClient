using RicoClient.Scripts.Cards;
using RicoClient.Scripts.Decks;
using RicoClient.Scripts.Menu.Modals;
using RicoClient.Scripts.User;
using System;
using System.Collections.Generic;
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

        public bool IsNewDeck { get; set; }

        public uint DeckId { get; private set; }
        public List<DeckCardScript> DeckCards { get; private set; }
        public string DeckName { get { return _deckName.text; } 
            set { _deckName.text = value; } }

        [Inject]
        public void Initialize(CardsManager cards)
        {
            _cards = cards;
        }

        protected void OnEnable()
        {
            DeckCardScript.OnCardDelete += CardDeletion;
            BaseCardScript.OnCardLeftClick += CardAddFromCollection;
        }

        protected void OnDisable()
        {
            DeckCardScript.OnCardDelete -= CardDeletion;
            BaseCardScript.OnCardLeftClick -= CardAddFromCollection;

            foreach (var deckCard in DeckCards)
            {
                Destroy(deckCard.gameObject);
            }

            DeckCards.Clear();
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
            DeckCards = new List<DeckCardScript>(deck.DeckCards.Count);
            IsNewDeck = false;

            foreach (var deckCard in deck.DeckCards)
                for (int i = 0; i < deckCard.Value; i++)
                    AddCardInDeck(deckCard.Key);
        }

        public void SetDeck()
        {
            DeckId = 0;
            DeckName = "New Deck";
            DeckCards = new List<DeckCardScript>();
            IsNewDeck = true;
        }

        public void AddCardInDeck(int cardId)
        {
            if (UserManager.PlayerCards.TryGetValue(cardId, out int ownedAmount))
            {
                int cardIndex = FindCardInDeck(cardId);
                if (cardIndex != -1 && ownedAmount > DeckCards[cardIndex].Amount)
                {
                    DeckCards[cardIndex].IncreaseDeckCardAmount();
                }
                else if (cardIndex == -1)
                {
                    var card = _cards.GetCardById(cardId);
                    var deckCard = Instantiate(_deckCardPrefab.gameObject, _deckContent.transform).GetComponent<DeckCardScript>();
                    deckCard.SetDeckCard(cardId, card.Name, card.Cost);

                    DeckCards.Add(deckCard);
                }
            }
        }

        private void CardDeletion(DeckCardScript deckCard)
        {
            for (int i = 0; i < DeckCards.Count; i++)
            {
                if (deckCard.CardId == DeckCards[i].CardId)
                {
                    DeckCards.RemoveAt(i);
                    break;
                }
            }

            Destroy(deckCard.gameObject);
        }

        private void CardAddFromCollection(BaseCardScript card)
        {
            AddCardInDeck(card.CardId);
        }

        private int FindCardInDeck(int cardId)
        {
            for (int i = 0; i < DeckCards.Count; i++)
                if (cardId == DeckCards[i].CardId)
                    return i;

            return -1;
        }
    }
}
