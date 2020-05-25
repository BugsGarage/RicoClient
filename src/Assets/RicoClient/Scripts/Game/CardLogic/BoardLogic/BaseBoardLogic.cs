using RicoClient.Scripts.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;

namespace RicoClient.Scripts.Game.CardLogic.BoardLogic
{
    public class BaseBoardLogic : BaseLogic
    {
        protected Image _highlightImage;

        public BaseBoardLogic(BaseCardScript card) : base(card)
        {
            _highlightImage = card.transform.parent.GetComponent<Image>();
        }
    }
}
