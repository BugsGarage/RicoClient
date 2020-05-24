using RicoClient.Scripts.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RicoClient.Scripts.Game.CardLogic
{
    public abstract class BaseLogic
    {
        public BaseCardScript CardScript { get { return _card.GetComponent<BaseCardScript>(); } }

        protected GameObject _card;

        protected CanvasGroup _canvasGroup;
        protected RectTransform _rectTransform;

        public BaseLogic(BaseCardScript card)
        {
            _card = card.gameObject;

            _canvasGroup = card.GetComponent<CanvasGroup>();
            _rectTransform = card.GetComponent<RectTransform>();
        }

        public virtual void OnRightClick()
        {
            Debug.Log("Default right click action");
        }

        public virtual void OnLeftClick()
        {
            Debug.Log("Default left click action");
        }

        public virtual void OnEnter()
        {
            Debug.Log("Default pointer enter action");
        }

        public virtual void OnExit()
        {
            Debug.Log("Default pointer exit action");
        }

        public virtual void OnBeginDrag()
        {
            Debug.Log("Default begin drag action");
        }

        public virtual void OnEndDrag()
        {
            Debug.Log("Default end drag action");
        }

        public virtual void OnDrag(Vector2 delta)
        {
            Debug.Log("Default drag action");
        }
    }
}
