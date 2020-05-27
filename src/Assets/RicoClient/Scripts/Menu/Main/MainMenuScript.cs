using RicoClient.Scripts.Exceptions;
using RicoClient.Scripts.Menu.Collection;
using RicoClient.Scripts.Menu.Play;
using RicoClient.Scripts.Menu.Shop;
using UniRx.Async;
using UnityEngine;

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
            bool res = await UpdatePlayer();

            if (res)
            {
                _collectionMenu.ReturnMenu = this;
                _collectionMenu.gameObject.SetActive(true);
                gameObject.SetActive(false);
            }
        }

        public async void OnShopClick()
        {
            bool res = await UpdatePlayer();

            if (res)
            {
                _shopMenu.gameObject.SetActive(true);
                gameObject.SetActive(false);
            }
        }

        public async void OnPlayClick()
        {
            bool res = await UpdatePlayer();

            if (res)
            {
                _playMenu.gameObject.SetActive(true);
                gameObject.SetActive(false);
            }
        }

        public void OnCloseClick()
        {
            Application.Quit();
        }

        private async UniTask<bool> UpdatePlayer()
        {
            try
            {
                await _user.UpdatePlayerInfo();
            }
            catch (PlayersException e)
            {
                Debug.LogError(e.Message);

                return false;
            }

            return true;
        }
    }
}
