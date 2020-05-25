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
        protected LineRenderer _aimLine;
        protected Image _highlightImage;

        public BaseBoardLogic(BaseCardScript card, LineRenderer aimLine) : base(card)
        {
            _aimLine = aimLine;
            _highlightImage = card.transform.parent.GetComponent<Image>();
        }

        public void HighlightCard()
        {
            _highlightImage.enabled = true;
        }

        public void UnhighlightCard()
        {
            _highlightImage.enabled = false;
        }
    }
}
