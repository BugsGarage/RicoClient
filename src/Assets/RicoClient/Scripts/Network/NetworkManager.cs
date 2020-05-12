﻿using RicoClient.Scripts.Cards;
using RicoClient.Scripts.Cards.Entities;
using RicoClient.Scripts.Exceptions;
using RicoClient.Scripts.Network.Controllers;
using RicoClient.Scripts.Network.Entities;
using RicoClient.Scripts.User;
using RicoClient.Scripts.User.Storage;
using System;
using System.Collections.Generic;
using UniRx.Async;
using UnityEngine;

namespace RicoClient.Scripts.Network
{
    public class NetworkManager
    {
        private readonly AuthController _authController;
        private readonly CardsController _cardsController;
        private readonly PlayerController _playerController;

        public NetworkManager(AuthController authController, CardsController cardsController, PlayerController playerController)
        {
            _authController = authController;
            _cardsController = cardsController;
            _playerController = playerController;
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
            return await _cardsController.GetAllCardsRequest(UserManager.FullAccessToken);
        }

        /// <summary>
        /// Get current player info
        /// </summary>
        /// <returns>Player info about his cards and decks</returns>
        public async UniTask<PlayerData> GetPlayerInfo()
        {
            return await _playerController.GetPlayerInfoRequest(UserManager.FullAccessToken);
        }
    }
}
