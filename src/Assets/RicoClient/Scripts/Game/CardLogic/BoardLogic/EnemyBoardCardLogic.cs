using RicoClient.Scripts.Cards;
using RicoClient.Scripts.Game.CardLogic.BoardLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RicoClient.Scripts.Game.CardLogic.BoardLogic
{
    public class EnemyBoardCardLogic : BaseBoardLogic
    {
        public EnemyBoardCardLogic(BaseCardScript card, LineRenderer aimLine) : base(card, aimLine)
        {

        }
    }
}
