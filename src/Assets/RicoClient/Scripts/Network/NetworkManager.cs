using RicoClient.Scripts.Cards.Entities;
using RicoClient.Scripts.Decks;
using RicoClient.Scripts.Exceptions;
using RicoClient.Scripts.Network.Controllers;
using RicoClient.Scripts.Network.Entities;
using RicoClient.Scripts.User;
using RicoClient.Scripts.User.Storage;
using System.Collections.Generic;
using UniRx.Async;

namespace RicoClient.Scripts.Network
{
    public class NetworkManager
    {
        private readonly AuthController _authController;
        private readonly CardsController _cardsController;
        private readonly PlayerController _playerController;
        private readonly PayController _payController;
        private readonly GameController _gameController;

        public NetworkManager(AuthController authController, CardsController cardsController, PlayerController playerController, 
            PayController payController, GameController gameController)
        {
            _authController = authController;
            _cardsController = cardsController;
            _playerController = playerController;
            _payController = payController;
            _gameController = gameController;
        }

        /// <summary>
        /// OAuth request
        /// </summary>
        /// <returns>Tokens</returns>
        public async UniTask<TokenInfo> OAuth()
        {
            try
            {
                TokenInfo tokens = await _authController.OAuthRequest();
                return tokens;
            }
            catch (OAuthException)
            {
                throw;
            }
        }

        /// <summary>
        /// Request for game entering
        /// </summary>
        public async UniTask EnterGame()
        {
            try
            {
                await _playerController.PostEnterGame(UserManager.FullAccessToken);
            }
            catch (PlayersException)
            {
                throw;
            }
        }

        /// <summary>
        /// Refresh token request
        /// </summary>
        /// <param name="refresh_token"></param>
        /// <returns>Tokens</returns>
        public async UniTask<TokenInfo> RefreshTokens(string refresh_token)
        {
            try
            {
                TokenInfo tokens = await _authController.RefreshTokenRequest(refresh_token);
                return tokens;
            }
            catch (OAuthException)
            {
                // Cause it calls usually from non-unity thread we should not handle exception here
                throw;  
            }
        }

        /// <summary>
        /// Get all in-game cards from server
        /// </summary>
        /// <returns>List with all in-game cards</returns>
        public async UniTask<List<Card>> GetAllCards()
        {
            try
            {
                return await _cardsController.GetAllCardsRequest();
            }
            catch (CardsException)
            {
                throw;
            }
        }

        /// <summary>
        /// Get current player info
        /// </summary>
        /// <returns>Player info about his cards and decks</returns>
        public async UniTask<PlayerData> GetPlayerInfo()
        {
            try
            {
                return await _playerController.GetPlayerInfoRequest(UserManager.FullAccessToken);
            }
            catch (PlayersException)
            {
                throw;
            }
        }

        /// <summary>
        /// Get deck by its id
        /// </summary>
        /// <returns>Deck information</returns>
        public async UniTask<Deck> GetDeckById(uint deckId)
        {
            try
            {
                return await _playerController.GetDeckByIdRequest(UserManager.FullAccessToken, deckId);
            }
            catch (PlayersException)
            {
                throw;
            }
        }

        /// <summary>
        /// Send just created deck to the server
        /// </summary>
        /// <returns>Created deck id</returns>
        public async UniTask<uint> PostNewDeck(string deckName, Dictionary<int, int> deckCards, int cardsCount)
        {
            Deck data = new Deck()
            {
                DeckId = 0,
                DeckName = deckName,
                CardsCount = cardsCount,
                DeckCards = deckCards
            };

            try
            {
                return await _playerController.PostNewDeckRequest(UserManager.FullAccessToken, data);
            }
            catch (PlayersException)
            {
                throw;
            }
        }

        /// <summary>
        /// Save changes of user's deck by its id
        /// </summary>
        /// <returns></returns>
        public async UniTask PatchDeckById(uint deckId, string deckName, Dictionary<int, int> deckCards, int cardsCount)
        {
            Deck data = new Deck()
            {
                DeckId = deckId,
                DeckName = deckName,
                CardsCount = cardsCount,
                DeckCards = deckCards
            };

            try
            {
                await _playerController.PatchDeckByIdRequest(UserManager.FullAccessToken, deckId, data);
            }
            catch (PlayersException)
            {
                throw;
            }
        }

        /// <summary>
        /// Delete user's deck by its id
        /// </summary>
        /// <returns></returns>
        public async UniTask DeleteDeckById(uint deckId)
        {
            try
            {
                await _playerController.DeleteDeckByIdRequest(UserManager.FullAccessToken, deckId);
            }
            catch (PlayersException)
            {
                throw;
            }
        }

        /// <summary>
        /// Get random card cost in gold
        /// </summary>
        /// <returns>Random card cost</returns>
        public async UniTask<int> GetRandomCardCost()
        {
            try
            { 
                return await _payController.GetRandomCardCostRequest();
            }
            catch (ShopException)
            {
                throw;
            }
        }

        /// <summary>
        /// Get coeff for converting real value in in-game gold
        /// </summary>
        /// <returns>Coeff</returns>
        public async UniTask<double> GetGoldCoeff()
        {
            try
            {
                return await _payController.GetGoldCoefRequest();
            }
            catch (PaymentException)
            {
                throw;
            }
        }

        /// <summary>
        /// Buy specific card for player
        /// </summary>
        /// <returns></returns>
        public async UniTask PostBuySpecificCard(int cardId)
        {
            try
            {
                await _payController.PostBuySpecificCardRequest(UserManager.FullAccessToken, cardId);
            }
            catch (ShopException)
            {
                throw;
            }
        }

        /// <summary>
        /// Buy random card for player
        /// </summary>
        /// <returns>Bought card id</returns>
        public async UniTask<int> PostBuyRandomCard()
        {
            try
            {
                return await _payController.PostBuyRandomCardRequest(UserManager.FullAccessToken);
            }
            catch (ShopException)
            {
                throw;
            }
        }

        /// <summary>
        /// Buy gold for real money (mock)
        /// </summary>
        /// <returns>Result of operation</returns>
        public async UniTask PostBuyGold(int value)
        {
            try
            {
                await _payController.PostBuyGoldRequest(UserManager.FullAccessToken, value);
            }
            catch (PaymentException)
            {
                throw;
            }
        }

        /// <summary>
        /// Connect to game request
        /// </summary>
        public async UniTask PostConnectGame(uint deckId)
        {
            try
            {
                await _gameController.PostConnectGameRequest(UserManager.FullAccessToken, deckId);
            }
            catch (GameException)
            {
                throw;
            }
        }
    }
}
