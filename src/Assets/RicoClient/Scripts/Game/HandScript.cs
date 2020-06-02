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
        [SerializeField]
        private float _baseSpacing = 0;

        private HorizontalLayoutGroup _horizontalGroup;
        private CanvasGroup _canvasGroup;

        protected void Awake()
        {
            _horizontalGroup = GetComponent<HorizontalLayoutGroup>();
            _canvasGroup = GetComponent<CanvasGroup>();
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

        public void PlayCardFromHand()
        {
            var playedCard = transform.GetChild(0);
            Destroy(playedCard.gameObject);

            RecalculateSpacing(1);
        }

        public void RecalculateSpacing(int isAfterDelete = 0)
        {
            int realChildCount = transform.childCount - isAfterDelete;

            float currAllCardsWidth = 0;
            for (int i = 0; i < realChildCount; i++)
            {
                RectTransform child = (RectTransform) transform.GetChild(i);
                currAllCardsWidth += child.sizeDelta.x;
            }

            float horizontalGroupWidth = ((RectTransform) _horizontalGroup.transform).rect.width;
            float newSpacing = -(currAllCardsWidth - horizontalGroupWidth) / (realChildCount - 1);
            if (_baseSpacing < newSpacing)
                _horizontalGroup.spacing = _baseSpacing;
            else
                _horizontalGroup.spacing = newSpacing;
        }

        private void AddCardInHand(GameObject card)
        {
            var cardHolder = Instantiate(_inHandCardHolder, transform);
            card.transform.SetParent(cardHolder.transform, false);

            Vector2 size = ((RectTransform) cardHolder.transform).sizeDelta;
            // Hack because card scales in a weird way without it
            card.transform.localPosition = Vector3.zero;
            ((RectTransform) card.transform).sizeDelta = size;

            RecalculateSpacing();
        }
    }
}
