using RicoClient.Scripts.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace RicoClient.Scripts.Game.CardLogic.BoardLogic
{
    public class BaseBoardLogic : BaseLogic
    {
        public static event Action<Vector3> OnBoardCardEnter;
        public static event Action OnBoardCardExit;
        public static event Action<BaseCardScript> OnDroppedOnCard;

        private readonly Color SelectionColor = new Color(0.16f, 0.47f, 1, 0.59f);
        private readonly Color HighlightColor = new Color(1, 1, 1, 0.59f);

        protected LineRenderer _aimLine;
        protected Image _highlightImage;

        public BaseBoardLogic(BaseCardScript card, LineRenderer aimLine) : base(card)
        {
            _aimLine = aimLine;
            _highlightImage = card.transform.parent.GetComponent<Image>();
        }

        public void HighlightCard()
        {
            _highlightImage.color = HighlightColor;
            _highlightImage.enabled = true;    
        }

        public void UnhighlightCard()
        {
            _highlightImage.enabled = false;
        }

        public override void OnRightClick()
        {
            Debug.Log("Board right click");
        }

        public override void OnEnter()
        {
            if (_highlightImage.enabled)
            {
                _highlightImage.color = SelectionColor;

                Vector3[] corners = new Vector3[4];
                _rectTransform.GetWorldCorners(corners);
                Vector3 downCardSide = new Vector3((corners[0].x + corners[3].x) / 2, corners[0].y, corners[0].z);

                OnBoardCardEnter?.Invoke(downCardSide);
            }
        }

        public override void OnExit()
        {
            if (_highlightImage.enabled)
            {
                _highlightImage.color = HighlightColor;

                OnBoardCardExit?.Invoke();
            }
        }

        public override void OnDrop()
        {
            if (_highlightImage.enabled)
            {
                OnDroppedOnCard?.Invoke(CardScript);
            }
        }
    }
}
