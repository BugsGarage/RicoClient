using RicoClient.Scripts.Menu.Collection;
using RicoClient.Scripts.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace RicoClient.Scripts.Menu.Main
{
    public class MainMenuScript : MonoBehaviour
    {
        private UserManager _user;

        [SerializeField]
        private CollectionMenuScript _collectionMenu = null;

        [Inject]
        public void Initialize(UserManager user)
        {
            _user = user;
        }

        public void SetMainMenuActive()
        {
            gameObject.SetActive(true);
        }

        public async void OnCollectionClick()
        {
            gameObject.SetActive(false);

            // try ?
            await _user.UpdatePlayerInfo();

            _collectionMenu.gameObject.SetActive(true);
        }
    }
}
