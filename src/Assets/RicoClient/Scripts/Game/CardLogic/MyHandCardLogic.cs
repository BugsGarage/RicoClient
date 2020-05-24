using RicoClient.Scripts.Cards;
using System;
using UnityEngine;

namespace RicoClient.Scripts.Game.CardLogic
{
    public class MyHandCardLogic : BaseLogic
    {
        public static event Action<BaseCardScript> OnCardSelected;

        private const float HighlightScale = 1.25f;
        private const float HighlightYShift = 160;
        private const int SortOrder = 1;

        private Canvas _cardCanvas;

        public MyHandCardLogic(BaseCardScript card) : base(card)
        {
            _cardCanvas = _card.GetComponent<Canvas>();

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

        public override void OnBeginDrag()
        {
            _canvasGroup.blocksRaycasts = false;

            OnCardSelected?.Invoke(CardScript);
        }
    }
}
