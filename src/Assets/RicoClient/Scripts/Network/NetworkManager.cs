using RicoClient.Scripts.Network.Controllers;
using RicoClient.Scripts.User;
using RicoClient.Scripts.User.Storage;
using UnityEngine;

namespace RicoClient.Scripts.Network
{
    public class NetworkManager
    {
        private readonly UserManager _userManager;

        private readonly AuthController _authController;

        public NetworkManager(UserManager userManager, AuthController authController)
        {
            _userManager = userManager;
            _authController = authController;
        }

        public async void OAuth()
        {
            TokenInfo tokens = await _authController.OAuthRequest();
            _userManager.AuthorizeUser(tokens);
        }
    }
}
