using RicoClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RicoClient.Configs
{
    public class PayConfig
    {
        public string ShopServerURL { get; }
        public string PaymentServerURL { get; }

        public PayConfig(AppConfig config)
        {
            ShopServerURL = config.ShopServerURL;
            PaymentServerURL = config.PaymentServerURL;
        }
    }
}
