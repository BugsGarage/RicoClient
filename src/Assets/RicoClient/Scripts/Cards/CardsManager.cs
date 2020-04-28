using RicoClient.Scripts.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RicoClient.Scripts.Cards
{
    public class CardsManager
    {
        private NetworkManager _network;

        public CardsManager(NetworkManager network)
        {
            _network = network;
        }

        /// <summary>
        /// Updating local cards database from cards server
        /// </summary>
        public void UpdateLocalCardsDB()
        {

        }
    }
}
