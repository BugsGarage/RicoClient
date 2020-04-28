using RicoClient.Configs;
using RicoClient.Scripts.Network.Controllers;
using UnityEngine;
using Zenject;

namespace RicoClient.Installers
{
    /// <summary>
    /// Installer for all in-project newtwork controllers
    /// </summary>
    public class ControllersInstaller : Installer<AppConfig, ControllersInstaller>
    {
        [Inject]
        private AppConfig _configuration = null;

        public override void InstallBindings()
        {
            Container.BindInstance(new AuthorizationConfig(_configuration)).AsSingle().NonLazy();
            Container.BindInstance(new CardsConfig(_configuration)).AsSingle().NonLazy();

            Container.Bind<AuthController>().AsSingle().NonLazy();
            Container.Bind<CardsController>().AsSingle().NonLazy();
        }
    }
}