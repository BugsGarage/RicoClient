using RicoClient.Scripts.Menu.Main;
using RicoClient.Scripts.Menu.Modals;
using RicoClient.Scripts.Network;
using UnityEngine;
using Zenject;

namespace RicoClient.Scripts.Menu.Auth
{
    public class AuthorizationMenuScript : MonoBehaviour
    {
        private NetworkManager _network;

        [SerializeField]
        private ModalInfo _modalInfo = null;
        [SerializeField]
        private MainMenuScript _mainMenu = null;

        [Inject]
        public void Initialize(NetworkManager network)
        {
            _network = network;
        }

        public async void OnAuthClick()
        {
            bool res = await _network.OAuth();
            if (res)
            {
                _modalInfo.SetInfoDialog("You've been completly authorized!", _mainMenu.SetMainMenuActive);
                gameObject.SetActive(false);
            }
        }
    }
}