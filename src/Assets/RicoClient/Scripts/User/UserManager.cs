using Assets.RicoClient.Scripts.Utils;
using RicoClient.Scripts.Cards;
using RicoClient.Scripts.Decks;
using RicoClient.Scripts.Exceptions;
using RicoClient.Scripts.Network;
using RicoClient.Scripts.Network.Entities;
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
        /// Builds the whole authorization header (token_type + access_token)
        /// </summary>
        public static string FullAccessToken { get { return $"{_userStorage.Tokens.TokenType} {_userStorage.Tokens.AccessToken}"; } } 

        /// <summary>
        /// Cards owned by player and their amount
        /// </summary>
        public static SortedDictionary<int, int> PlayerCards { get { return _userStorage.OwnedCards; } }

        /// <summary>
        /// Player's deck headers
        /// </summary>
        public static List<DeckHeader> DeckHeaders { get { return _userStorage.Decks; } }

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

            if (string.IsNullOrEmpty(tokens.AccessToken))
                throw new AuthorizeException("Authorization timeout!");

            var decodedJWT = JWTDecoder.Decode(tokens.AccessToken);
            if (decodedJWT == null)
                throw new AuthorizeException("Wrong JWT recieved!");

            _userStorage.Username = decodedJWT["name"].ToString();
            UpdateTokens(tokens);

            try
            {
                await _network.EnterGame();
            }
            catch (PlayersException e)
            {
                throw new AuthorizeException("Can't enter game!", e);
            }

            try
            {
                await _cards.UpdateLocalCards();
            }
            catch (CardsException e)
            {
                throw new AuthorizeException("Can't get cards for play!", e);
            }
        }

        public async UniTask UpdatePlayerInfo()
        {
            PlayerData data;
            try
            {
                data = await _network.GetPlayerInfo();
            }
            catch (PlayersException)
            {
                throw;
            }

            _userStorage.BalanceValue = data.Balance;
            _userStorage.OwnedCards = new SortedDictionary<int, int>(data.OwnedCards);
            _userStorage.Decks = data.Decks;
        }

        public void RefillBalance(int value)
        {
            _userStorage.BalanceValue += value;
        }

        public void BuyLocalCard(int cardId, int cardCost)
        {
            _userStorage.BalanceValue -= cardCost;
            if (_userStorage.OwnedCards.TryGetValue(cardId, out _))
            {
                _userStorage.OwnedCards[cardId] += 1;
            }
            else
            {
                _userStorage.OwnedCards.Add(cardId, 1);
            }
        }

        public void UpdateLocalDeck(uint deckId, string deckName, int cardsCount)
        {
            for (int i = 0; i < _userStorage.Decks.Count; i++)
            {
                if (deckId == _userStorage.Decks[i].DeckId)
                {
                    _userStorage.Decks[i].DeckName = deckName;
                    _userStorage.Decks[i].CardsCount = cardsCount;
                    return;
                }
            }

            // If deck has just created
            _userStorage.Decks.Add(new DeckHeader() { DeckId = deckId, DeckName = deckName, CardsCount = cardsCount });
        }

        public void DeleteLocalDeck(uint deckId)
        {
            for (int i = 0; i < _userStorage.Decks.Count; i++)
                if (deckId == _userStorage.Decks[i].DeckId)
                    _userStorage.Decks.RemoveAt(i);
        }

        private void UpdateTokens(TokenInfo tokens)
        {
            _userStorage.Tokens = tokens;
            RefreshTokensSetup(tokens.RefreshToken, tokens.ExpiresIn);
        }

        private void RefreshTokensSetup(string refresh_token, int expires_in)
        {
            int expires_inMs = (expires_in - 10) * 1000;
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
