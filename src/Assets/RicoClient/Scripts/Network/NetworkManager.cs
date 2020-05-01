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
        private readonly UserManager _userManager;

        private readonly AuthController _authController;
        private readonly CardsController _cardsController;

        public NetworkManager(UserManager userManager, AuthController authController, CardsController cardsController)
        {
            _userManager = userManager;

            _authController = authController;
            _cardsController = cardsController;
        }

        public async UniTask<bool> OAuth()
        {
            try
            {
                TokenInfo tokens = await _authController.OAuthRequest();
                if (tokens.AccessToken != null && tokens.AccessToken.Length > 0)
                    _userManager.AuthorizeUser(tokens);
            }
            catch (RicoException e)
            {
                Debug.LogError(e.Message);
                return false;
            }

            RefreshTokensSetup();
            return true;
        }

        public void RefreshTokensSetup()
        {
            TokenInfo currTokenInfo = _userManager.GetTokensInfo();
            int expires_inMs = (currTokenInfo.ExpiresIn - 5) * 1000;
            RefreshTokens(currTokenInfo.RefreshToken, expires_inMs).Forget((Exception e) => Debug.LogError(e.Message));
        }

        public async UniTask<bool> GetAllCards()
        {
            _cardsController.GetAllCardsRequest();

            return true;
        }

        private async UniTask RefreshTokens(string refresh_token, int expires_inMs)
        {
            await UniTask.Delay(expires_inMs);

            TokenInfo tokens = await _authController.RefreshTokenRequest(refresh_token);
            if (tokens.AccessToken != null && tokens.AccessToken.Length > 0)
                _userManager.AuthorizeUser(tokens);

            RefreshTokensSetup();
        }
    }
}
