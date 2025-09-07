using ScenarioToolkit.Library.Systems;
using Zenject;

namespace ScenarioToolkit.Library.Installers
{
    public class RandomSystemInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<RandomSystem>().AsSingle().NonLazy();
        }
    }
}