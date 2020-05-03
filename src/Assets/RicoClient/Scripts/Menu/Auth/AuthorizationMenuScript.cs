using RicoClient.Scripts.Menu.Main;
using RicoClient.Scripts.Menu.Modals;
using RicoClient.Scripts.User;
using UnityEngine;
using Zenject;

namespace RicoClient.Scripts.Menu.Auth
{
    public class AuthorizationMenuScript : MonoBehaviour
    {
        private UserManager _user;

        [SerializeField]
        private ModalInfo _modalInfo = null;
        [SerializeField]
        private MainMenuScript _mainMenu = null;

        [Inject]
        public void Initialize(UserManager user)
        {
            _user = user;
        }

        public async void OnAuthClick()
        {
            gameObject.SetActive(false);

            bool res = await _user.AuthorizeUser();
            if (res)
            {
                _modalInfo.SetInfoDialog("You've been completly authorized!", _mainMenu.SetMainMenuActive);
            }
            else
            {
                gameObject.SetActive(true);
            }
        }
    }
}