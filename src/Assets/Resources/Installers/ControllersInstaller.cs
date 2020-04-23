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
            Container.BindInstance(_configuration).AsSingle().NonLazy();

            Container.Bind<AuthController>().AsSingle().NonLazy();
        }
    }
}