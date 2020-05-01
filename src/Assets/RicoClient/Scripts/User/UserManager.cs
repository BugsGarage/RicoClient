using Assets.RicoClient.Scripts.Utils;
using RicoClient.Scripts.Exceptions;
using RicoClient.Scripts.Network;
using RicoClient.Scripts.User.Storage;
using System;
using System.Timers;
using UniRx.Async;
using UnityEngine;

namespace RicoClient.Scripts.User
{
    public class UserManager
    {
        private UserStorage _userStorage;

        public UserManager()
        {
            _userStorage = new UserStorage();
        }

        public void AuthorizeUser(TokenInfo tokens)
        {
            var decodedJWT = JWTDecoder.Decode(tokens.AccessToken);
            if (decodedJWT == null)
                throw new AuthorizeException("Wrong JWT recieved!");

            _userStorage.Username = decodedJWT["name"].ToString();
            _userStorage.Tokens = tokens;
        }

        public TokenInfo GetTokensInfo()
        {
            return _userStorage.Tokens;
        }
    }
}
