using ScenarioToolkit.Library.Systems;
using Zenject;

namespace ScenarioToolkit.Library.Installers
{
    public class ParentingObjectSystemInstaller : MonoInstaller
    {
        public override void InstallBindings() =>
            Container.BindInterfacesAndSelfTo<ParentingObjectSystem>()
                .AsSingle()
                .NonLazy();
    }
}