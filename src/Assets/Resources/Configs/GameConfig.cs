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
        public string GameServerURL { get; }

        public GameConfig(AppConfig config)
        {
            GameServerURL = config.GameServerURL;
        }
    }
}
