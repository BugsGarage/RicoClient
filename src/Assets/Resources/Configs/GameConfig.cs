using RicoClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RicoClient.Configs
{
    public class GameConfig
    {
        public string GameConnectEndpoint { get; }

        public string GameWebsocketPath { get; }

        public GameConfig(AppConfig config)
        {
            GameConnectEndpoint = config.GameConnectEndpoint;
            GameWebsocketPath = config.GameWebsocketPath;
        }
    }
}
