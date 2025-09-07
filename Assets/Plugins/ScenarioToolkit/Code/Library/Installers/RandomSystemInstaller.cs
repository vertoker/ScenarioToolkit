using Scenario.Systems;
using Zenject;

namespace Scenario.Installers
{
    public class RandomSystemInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<RandomSystem>().AsSingle().NonLazy();
        }
    }
}