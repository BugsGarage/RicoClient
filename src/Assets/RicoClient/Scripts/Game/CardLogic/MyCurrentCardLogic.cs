using RicoClient.Scripts.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RicoClient.Scripts.Game.CardLogic
{
    public class MyCurrentCardLogic : BaseLogic
    {
        public static event Action<BaseCardScript> CardReturnedToHand;

        public GameObject InHandCardHolder { get; private set; }

        public MyCurrentCardLogic(BaseCardScript card, Transform parent) : base(card)
        {
            InHandCardHolder = _rectTransform.parent.gameObject;
            _rectTransform.SetParent(parent, true);
        }

        public override void OnDrag(Vector2 delta)
        {
            _rectTransform.anchoredPosition += delta;
        }

        public override void OnEndDrag()
        {
            _canvasGroup.blocksRaycasts = true;

            _rectTransform.SetParent(InHandCardHolder.transform, false);
            CardScript.PlaceInHand();

            CardReturnedToHand?.Invoke(CardScript); 
        }
    }
}
