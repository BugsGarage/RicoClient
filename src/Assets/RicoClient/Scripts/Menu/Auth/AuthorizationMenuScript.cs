using RicoClient.Scripts.Exceptions;
using RicoClient.Scripts.Menu.Main;
using RicoClient.Scripts.Menu.Modals;
using RicoClient.Scripts.User;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace RicoClient.Scripts.Menu.Auth
{
    public class AuthorizationMenuScript : BaseMenuScript
    {
        [SerializeField]
        private ModalInfo _modalInfo = null;
        [SerializeField]
        private MainMenuScript _mainMenu = null;
        [SerializeField]
        private Button _authButton = null;

        public async void OnAuthClick()
        {
            _authButton.interactable = false;

            try
            {
                await _user.AuthorizeUser();
            }
            catch (AuthorizeException e)
            {
                Debug.LogError(e.Message + '\n' + e.InnerException.Message);
                _authButton.interactable = true;

                return;
            }

            _modalInfo.SetInfoDialog("You've been completly authorized!", new Action(() => { 
                _mainMenu.gameObject.SetActive(true);  
                gameObject.SetActive(false); 
            }));
        }

        public void OnCloseClick()
        {
            Application.Quit();
        }
    }
}