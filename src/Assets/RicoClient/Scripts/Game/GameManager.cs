using RicoClient.Scripts.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RicoClient.Scripts.Game
{
    public class GameManager
    {
        private readonly NetworkManager _network;

        public GameManager(NetworkManager network)
        {
            _network = network;
        }
    }
}
