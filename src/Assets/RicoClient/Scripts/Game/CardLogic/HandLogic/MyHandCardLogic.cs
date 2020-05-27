using RicoClient.Scripts.Cards;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RicoClient.Scripts.Game.CardLogic.HandLogic
{
    public class MyHandCardLogic : BaseHandLogic
    {
        public static event Action<BaseCardScript> OnCardSelected;

        private const float HighlightScale = 1.25f;
        private const float HighlightYShift = 160;
        private const int SortOrder = 1;

        public MyHandCardLogic(BaseCardScript card) : base(card)
        {
            // Clear everything in case after MyCurrentCardLogic
            OnExit();
        }

        public override void OnEnter()
        {
            _cardCanvas.overrideSorting = true;
            _cardCanvas.sortingOrder = SortOrder;

            _rectTransform.localScale = new Vector3(HighlightScale, HighlightScale);
            _rectTransform.localPosition = new Vector3(0, HighlightYShift);
        }

        public override void OnExit()
        {
            _cardCanvas.overrideSorting = false;

            _rectTransform.localScale = Vector3.one;
            _rectTransform.localPosition = Vector3.zero;
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            _canvasGroup.blocksRaycasts = false;
            _cardCanvas.overrideSorting = false;

            OnCardSelected?.Invoke(CardScript);
        }
    }
}
