using RicoClient.Scripts.Exceptions;
using RicoClient.Scripts.Network;
using RicoClient.Scripts.User;
using UniRx.Async;

namespace RicoClient.Scripts.Pay
{
    public class ShopManager
    {
        private readonly NetworkManager _network;
        private readonly UserManager _user;

        public ShopManager(NetworkManager network, UserManager user)
        {
            _network = network;
            _user = user;
        }

        public async UniTask<int> GetRandomCardCost()
        {
            try
            { 
                return await _network.GetRandomCardCost();
            }
            catch (ShopException)
            {
                throw;
            }
        }

        public async UniTask<double> GetGoldCoeff()
        {
            try
            {
                return await _network.GetGoldCoeff();
            }
            catch (PaymentException)
            {
                throw;
            }
        }

        public async UniTask BuySpecificCard(int cardId, int cardCost)
        {
            if (UserManager.Balance < cardCost)
                throw new NotEnoughBalanceException("Not enough gold!");

            try
            {
                await _network.PostBuySpecificCard(cardId);
            }
            catch (ShopException)
            {
                throw;
            }

            _user.BuyLocalCard(cardId, cardCost);
        }

        public async UniTask<int> BuyRandomCard(int randomCost)
        {
            if (UserManager.Balance < randomCost)
                throw new NotEnoughBalanceException("Not enough gold!");

            int cardId;
            try
            {
                cardId = await _network.PostBuyRandomCard();
            }
            catch (ShopException)
            {
                throw;
            }

            _user.BuyLocalCard(cardId, randomCost);

            return cardId;
        }

        public async UniTask BuyGold(int value)
        {
            try
            {
                // Super mock
                await _network.PostBuyGold(value);
            }
            catch (PaymentException)
            {
                throw;
            }

            _user.RefillBalance(value);
        }
    }
}
