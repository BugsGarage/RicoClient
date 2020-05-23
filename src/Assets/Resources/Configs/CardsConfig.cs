using RicoClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RicoClient.Configs
{
    public class CardsConfig
    {
        public string CardsServerURL { get; }

        public CardsConfig(AppConfig config)
        {
            CardsServerURL = config.CardsServerURL;
        }
    }
}
