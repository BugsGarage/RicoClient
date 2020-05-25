using RicoClient.Scripts.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RicoClient.Scripts.Game.CardLogic.HandLogic
{
    public class BaseHandLogic : BaseLogic
    {
        protected Canvas _cardCanvas;

        public BaseHandLogic(BaseCardScript card) : base(card)
        {
            _cardCanvas = _card.GetComponent<Canvas>();
        }
    }
}
