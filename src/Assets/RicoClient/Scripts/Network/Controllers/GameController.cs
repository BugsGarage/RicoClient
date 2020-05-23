using Newtonsoft.Json;
using RicoClient.Configs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx.Async;

namespace RicoClient.Scripts.Network.Controllers
{
    public class GameController
    {
        private readonly string _gameServerURL;

        public GameController(GameConfig configuration)
        {
            _gameServerURL = configuration.GameServerURL;
        }

        
    }
}
