using RicoClient.Scripts.Pay;
using RicoClient.Scripts.Cards;
using RicoClient.Scripts.Decks;
using RicoClient.Scripts.Network;
using RicoClient.Scripts.User;
using Zenject;

namespace RicoClient.Installers
{
    /// <summary>
    /// Installer for all in-project managers
    /// </summary>
    public class ManagersInstaller : Installer<ManagersInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<NetworkManager>().AsSingle().NonLazy();
            Container.Bind<CardsManager>().AsSingle().NonLazy();
            Container.Bind<PayManager>().AsSingle().NonLazy();
            Container.Bind<DeckManager>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<UserManager>().AsSingle().NonLazy();
        }
    }
}