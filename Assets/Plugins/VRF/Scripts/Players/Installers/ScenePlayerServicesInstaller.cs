using VRF.Players.Services.Views;
using Zenject;

namespace VRF.Players.Installers
{
    public class ScenePlayerServicesInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<SceneViewSpawnerService>().AsSingle();
        }
    }
}