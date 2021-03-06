using RicoClient.Scripts.Network.Controllers;
using Zenject;

namespace RicoClient.Installers
{
    /// <summary>
    /// Installer for all in-project newtwork controllers
    /// </summary>
    public class ControllersInstaller : Installer<ControllersInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<AuthController>().AsSingle().NonLazy();
            Container.Bind<CardsController>().AsSingle().NonLazy();
            Container.Bind<PlayerController>().AsSingle().NonLazy();
            Container.Bind<PayController>().AsSingle().NonLazy();
            Container.Bind<GameController>().AsSingle().NonLazy();
        }
    }
}