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
        public string GoldCoefEndpoint { get; }
        public string PaymentEndpoint { get; }

        public PayConfig(AppConfig config)
        {
            GoldCoefEndpoint = config.GoldCoefEndpoint;
            PaymentEndpoint = config.PaymentEndpoint;
        }
    }
}
