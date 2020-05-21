using RicoClient.Scripts.Menu.Collection;
using RicoClient.Scripts.Menu.Play;
using RicoClient.Scripts.Menu.Shop;
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
    public class MainMenuScript : BaseMenuScript
    {
        [SerializeField]
        private CollectionMenuScript _collectionMenu = null;
        [SerializeField]
        private ShopMenuScript _shopMenu = null;
        [SerializeField]
        private PlayMenuScript _playMenu = null;

        public void SetMainMenuActive()
        {
            gameObject.SetActive(true);
        }

        public async void OnCollectionClick()
        {
            // try ?
            await _user.UpdatePlayerInfo();

            _collectionMenu.ReturnMenu = this;
            _collectionMenu.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }

        public async void OnShopClick()
        {
            // try ?
            await _user.UpdatePlayerInfo();

            _shopMenu.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }

        public async void OnPlayClick()
        {
            // try ?
            await _user.UpdatePlayerInfo();

            _playMenu.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }

        public void OnCloseClick()
        {
            Application.Quit();
        }
    }
}
