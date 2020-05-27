using RicoClient.Scripts.Cards;
using RicoClient.Scripts.Cards.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace RicoClient.Scripts.Game
{
    public class HandScript : MonoBehaviour
    {
        [SerializeField]
        private GameObject _inHandCardHolder = null;

        public List<GameObject> InHandsCards { get; private set; }

        private HorizontalLayoutGroup _horizontalGroup;
        private CanvasGroup _canvasGroup;

        private float _currAllCardsWidth;

        protected void Awake()
        {
            InHandsCards = new List<GameObject>();

            _horizontalGroup = GetComponent<HorizontalLayoutGroup>();
            _canvasGroup = GetComponent<CanvasGroup>();
            _currAllCardsWidth = 0;
        }

        protected void OnDestroy()
        {
            foreach (var handCard in InHandsCards)
                Destroy(handCard.gameObject);
        }

        public void AddRevealedCardInHand(BaseCardScript card)
        {
            card.PlaceInHand();
            AddCardInHand(card.gameObject);
        }

        public void AddHiddenCardInHand(GameObject backCard)
        {
            AddCardInHand(backCard);
        }

        public void BlockHand()
        {
            _canvasGroup.blocksRaycasts = false;
        }

        public void EnableHand()
        {
            _canvasGroup.blocksRaycasts = true;
        }

        private void AddCardInHand(GameObject card)
        {
            var cardHolder = Instantiate(_inHandCardHolder, transform);
            card.transform.SetParent(cardHolder.transform, false);

            Vector2 size = ((RectTransform) cardHolder.transform).sizeDelta;
            // Hack because card scales in a weird way without it
            card.transform.localPosition = Vector3.zero;
            ((RectTransform) card.transform).sizeDelta = size;

            _currAllCardsWidth += size.x;
            float realAllCardsWidth = _currAllCardsWidth + _horizontalGroup.spacing * (InHandsCards.Count - 1);
            float horizontalGroupWidth = ((RectTransform) _horizontalGroup.transform).rect.width;
            if (realAllCardsWidth > horizontalGroupWidth)
            {
                _horizontalGroup.spacing = _horizontalGroup.spacing - (realAllCardsWidth - horizontalGroupWidth) / InHandsCards.Count;
            }

            InHandsCards.Add(card.gameObject);
        }
    }
}
