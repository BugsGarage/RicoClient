using RicoClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RicoClient.Configs
{
    public class ShopConfig
    {
        public string SpecificCardEndpoint { get; }
        public string RandomCardEndpoint { get; }
        public string RandomCostEndpoint { get; }

        public ShopConfig(AppConfig config)
        {
            SpecificCardEndpoint = config.SpecificCardEndpoint;
            RandomCardEndpoint = config.RandomCardEndpoint;
            RandomCostEndpoint = config.RandomCostEndpoint;
        }
    }
}
