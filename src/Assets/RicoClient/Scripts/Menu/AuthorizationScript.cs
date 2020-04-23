using RicoClient.Scripts.Network;
using UnityEngine;
using Zenject;

namespace RicoClient.Scripts.Menu
{
    public class AuthorizationScript : MonoBehaviour
    {
        private NetworkManager _network;

        [Inject]
        public void Initialize(NetworkManager network)
        {
            _network = network;
        }

        public void OnLoginClick()
        {
            _network.OAuth();
        }
    }
}