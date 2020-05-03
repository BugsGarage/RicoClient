using RicoClient.Scripts.Exceptions;
using RicoClient.Scripts.Network.Controllers;
using RicoClient.Scripts.User;
using RicoClient.Scripts.User.Storage;
using System;
using UniRx.Async;
using UnityEngine;

namespace RicoClient.Scripts.Network
{
    public class NetworkManager
    {
        private readonly AuthController _authController;
        private readonly CardsController _cardsController;

        public NetworkManager(AuthController authController, CardsController cardsController)
        {
            _authController = authController;
            _cardsController = cardsController;
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
            catch (OAuthException e)
            {
                Debug.LogError(e.Message);
                return new TokenInfo();
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

        public async UniTask<bool> GetAllCards()
        {
            _cardsController.GetAllCardsRequest();

            return true;
        }
    }
}
