using RicoClient.Scripts.Cards;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RicoClient.Scripts.Game.CardLogic.CurrentLogic
{
    public class MyCurrentCardLogic : BaseCurrentLogic
    {
        public static event Action<BaseCardScript> OnCardReturnedToHand;
        public static event Action<GameObject, BaseCardScript> OnCardDroppedToBoard;

        public MyCurrentCardLogic(BaseCardScript card, Transform parent) : base(card, parent)
        {
            
        }

        public override void OnDrag(PointerEventData eventData)
        {
            _rectTransform.anchoredPosition += eventData.delta;
        }

        public override void OnEndDrag()
        {
            _canvasGroup.blocksRaycasts = true;

            InHandCardHolder.SetActive(true);
            _rectTransform.SetParent(InHandCardHolder.transform, false);
            CardScript.PlaceInHand();

            OnCardReturnedToHand?.Invoke(CardScript); 
        }

        public override void CardDropped()
        {
            _canvasGroup.blocksRaycasts = true;

            OnCardDroppedToBoard?.Invoke(InHandCardHolder, CardScript);
        }
    }
}
