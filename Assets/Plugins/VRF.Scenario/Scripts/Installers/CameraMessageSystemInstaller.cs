using VRF.Scenario.Systems;
using Zenject;

namespace Modules.VRF.Scenario.Installers
{
    public class CameraMessageSystemInstaller : MonoInstaller
    {
        public override void InstallBindings() =>
            Container.Bind<CameraMessageSystem>()
                .AsSingle()
                .NonLazy();
    }
}