using RicoClient.Scripts.Exceptions;
using RicoClient.Scripts.Network;
using RicoClient.Scripts.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx.Async;

namespace RicoClient.Scripts.Pay
{
    public class PayManager
    {
        private readonly NetworkManager _network;
        private readonly UserManager _user;

        public PayManager(NetworkManager network, UserManager user)
        {
            _network = network;
            _user = user;
        }

        public async UniTask BuySpecificCard(int cardId, int cardCost)
        {
            if (UserManager.Balance < cardCost)
                throw new NotEnoughBalanceException("Not enough gold!");

            await _network.PostBuySpecificCard(cardId);
            _user.BuyLocalCard(cardId, cardCost);
        }
    }
}
