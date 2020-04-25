using Assets.RicoClient.Scripts.Utils;
using RicoClient.Scripts.Exceptions;
using RicoClient.Scripts.User.Storage;
using System;
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
            UpdateTokens(tokens);
        }

        public void UpdateTokens(TokenInfo tokens)
        {
            _userStorage.Tokens = tokens;
        }
    }
}
