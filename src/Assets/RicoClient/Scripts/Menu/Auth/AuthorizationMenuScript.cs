using RicoClient.Scripts.Exceptions;
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

            try
            {
                await _user.AuthorizeUser();
            }
            catch (AuthorizeException e)
            {
                Debug.LogError(e.Message);
                gameObject.SetActive(true);

                return;
            }

            _modalInfo.SetInfoDialog("You've been completly authorized!", _mainMenu.SetMainMenuActive);
        }
    }
}