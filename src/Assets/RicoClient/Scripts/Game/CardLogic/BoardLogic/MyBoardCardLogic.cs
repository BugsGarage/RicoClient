using RicoClient.Scripts.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RicoClient.Scripts.Game.CardLogic.BoardLogic
{
    public class MyBoardCardLogic : BaseBoardLogic
    {
        public static event Action<UnitCardScript> OnCardClicked;

        private bool _canAttack;

        public MyBoardCardLogic(BaseCardScript card) : base(card)
        {
            //_canAttack = false;
            _canAttack = true;
        }

        public override void OnLeftClick()
        {
            if (CardScript is UnitCardScript && _canAttack)
                OnCardClicked?.Invoke((UnitCardScript) CardScript);
        }

        public override void OnRightClick()
        {
            Debug.Log("Board right click");
        }
    }
}
