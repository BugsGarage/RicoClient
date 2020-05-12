using RicoClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RicoClient.Configs
{
    public class PlayerConfig
    {
        public string PlayerServerURL { get; }

        public PlayerConfig(AppConfig config)
        {
            PlayerServerURL = config.PlayerServerURL;
        }
    }
}
