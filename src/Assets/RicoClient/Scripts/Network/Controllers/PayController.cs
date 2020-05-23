using RicoClient.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx.Async;

namespace RicoClient.Scripts.Network.Controllers
{
    public class PayController
    {
        private readonly string _shopServerURL;
        private readonly string _paymentServerURL;

        public PayController(PayConfig configuration)
        {
            _shopServerURL = configuration.ShopServerURL;
            _paymentServerURL = configuration.PaymentServerURL;
        }

        public async UniTask<int> GetRandomCardCostRequest()
        {
            // ToDo: Request code

            return 100;
        }

        public async UniTask PostBuySpecificCardRequest(string access_token, int cardId)
        {
            // ToDo: Request code
        }

        public async UniTask<int> PostBuyRandomCardRequest(string access_token)
        {
            // ToDo: Request code

            return 7;
        }

        public async UniTask<bool> PostBuyGoldRequest(string access_token, int value)
        {
            // ToDo: Request code

            return true;
        }
    }
}
