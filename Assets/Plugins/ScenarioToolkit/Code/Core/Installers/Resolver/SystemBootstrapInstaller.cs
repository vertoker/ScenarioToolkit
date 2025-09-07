using Zenject;

namespace ScenarioToolkit.Core.Installers.Resolver
{
    public class SystemBootstrapInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<SystemBootstrap>().AsSingle();
        }
    }
}