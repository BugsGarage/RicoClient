using RicoClient.Scripts.Cards.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RicoClient.Scripts.Utils
{
    public class CardsComparer : IComparer<Card>
    {
        public int Compare(Card a, Card b)
        {
            if (a.CardId > b.CardId)
                return 1;
            else if (a.CardId < b.CardId)
                return -1;

            return 0;
        }
    }
}
