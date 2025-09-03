using Scenario.Systems;
using Zenject;

namespace Scenario.Installers
{
    public class ParentingObjectSystemInstaller : MonoInstaller
    {
        public override void InstallBindings() =>
            Container.BindInterfacesAndSelfTo<ParentingObjectSystem>()
                .AsSingle()
                .NonLazy();
    }
}