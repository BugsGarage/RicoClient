using RicoClient.Scripts.Cards;
using System;
using UnityEngine;

namespace RicoClient.Scripts.Game.CardLogic.CurrentLogic
{
    public class MyCurrentCardLogic : BaseCurrentLogic
    {
        public static event Action<BaseCardScript> OnCardReturnedToHand;
        public static event Action<GameObject> OnCardDroppedToBoard;

        public MyCurrentCardLogic(BaseCardScript card, Transform parent) : base(card, parent)
        {
            
        }

        public override void OnDrag(Vector2 delta)
        {
            _rectTransform.anchoredPosition += delta;
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

            OnCardDroppedToBoard?.Invoke(InHandCardHolder);
        }
    }
}
