using Assets.RicoClient.Scripts.Utils;
using RicoClient.Scripts.Cards;
using RicoClient.Scripts.Exceptions;
using RicoClient.Scripts.Network;
using RicoClient.Scripts.User.Storage;
using System;
using System.Collections.Generic;
using System.Threading;
using UniRx.Async;
using UnityEngine;

namespace RicoClient.Scripts.User
{
    public class UserManager : IDisposable
    {
        private static UserStorage _userStorage = new UserStorage();

        /// <summary>
        /// Builds the whole authorization header (type + access_token)
        /// </summary>
        public static string FullAccessToken { get { return $"{_userStorage.Tokens.TokenType} {_userStorage.Tokens.AccessToken}"; } } 

        /// <summary>
        /// Cards owned by player
        /// </summary>
        public static Dictionary<int, int> PlayerCards { get { return _userStorage.OwnedCards; } }

        /// <summary>
        /// Player balance
        /// </summary>
        public static int Balance { get { return _userStorage.BalanceValue; } }

        private CancellationTokenSource _cancellationToken;

        private readonly NetworkManager _network;
        private readonly CardsManager _cards;

        public UserManager(NetworkManager network, CardsManager cards)
        {
            _cancellationToken = new CancellationTokenSource();

            _network = network;
            _cards = cards;
        }

        /// <summary>
        /// OAuth request and tokens updating
        /// </summary>
        /// <returns>Result of the authorization</returns>
        public async UniTask AuthorizeUser()
        {
            TokenInfo tokens;
            try
            {
                tokens = await _network.OAuth();
            }
            catch (OAuthException e)
            {
                throw new AuthorizeException("Error during making OAuth!", e);
            }

            var decodedJWT = JWTDecoder.Decode(tokens.AccessToken);
            if (decodedJWT == null)
                throw new AuthorizeException("Wrong JWT recieved!");

            _userStorage.Username = decodedJWT["name"].ToString();
            UpdateTokens(tokens);

            await _cards.UpdateLocalCards();
        }

        public async UniTask UpdatePlayerInfo()
        {
            var data = await _network.GetPlayerInfo();

            _userStorage.BalanceValue = data.Balance;
            _userStorage.OwnedCards = data.OwnedCards;
            _userStorage.Decks = data.Decks;
        }

        private void UpdateTokens(TokenInfo tokens)
        {
            _userStorage.Tokens = tokens;
            RefreshTokensSetup(tokens.RefreshToken, tokens.ExpiresIn);
        }

        private void RefreshTokensSetup(string refresh_token, int expires_in)
        {
            int expires_inMs = (expires_in - 5) * 1000;
            RefreshTokens(refresh_token, expires_inMs).Forget((Exception e) => Debug.LogError(e.Message));
        }

        private async UniTask RefreshTokens(string refresh_token, int expires_inMs)
        {
            await UniTask.Delay(expires_inMs, cancellationToken: _cancellationToken.Token);

            TokenInfo tokens = await _network.RefreshTokens(refresh_token);
            if (tokens.AccessToken != null && tokens.AccessToken.Length > 0)
                UpdateTokens(tokens);
        }

        #region IDisposable Support

        private volatile bool _disposed = false; 

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _cancellationToken.Cancel();
                    _cancellationToken.Dispose();
                   
                }
                _cancellationToken = null;

                _disposed = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
