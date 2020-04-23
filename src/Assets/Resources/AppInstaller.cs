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
            ControllersInstaller.Install(Container, _configuration);
            ManagersInstaller.Install(Container);
        }
    }
}