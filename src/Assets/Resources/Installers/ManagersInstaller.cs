using RicoClient.Scripts.Network;
using RicoClient.Scripts.User;
using UnityEngine;
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
            Container.Bind<UserManager>().AsSingle().NonLazy();
        }
    }
}