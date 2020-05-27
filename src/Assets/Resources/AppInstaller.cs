using RicoClient.Configs;
using RicoClient.Installers;
using UnityEngine;
using Zenject;

namespace RicoClient
{
    /// <summary>
    /// Some kind of entry point to the app
    /// Main installer for every in-project installers
    /// </summary>
    public class AppInstaller : MonoInstaller
    {
        [SerializeField]
        [Header("Application configuration")]
        private AppConfig _configuration = null;

        public override void InstallBindings()
        {
            Container.BindInstance(new AuthorizationConfig(_configuration)).AsSingle().NonLazy();
            Container.BindInstance(new CardsConfig(_configuration)).AsSingle().NonLazy();
            Container.BindInstance(new PlayerConfig(_configuration)).AsSingle().NonLazy();
            Container.BindInstance(new PayConfig(_configuration)).AsSingle().NonLazy();
            Container.BindInstance(new ShopConfig(_configuration)).AsSingle().NonLazy();
            Container.BindInstance(new GameConfig(_configuration)).AsSingle().NonLazy();

            ControllersInstaller.Install(Container);
            ManagersInstaller.Install(Container);
        }
    }
}