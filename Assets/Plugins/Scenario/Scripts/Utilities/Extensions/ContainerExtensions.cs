using Scenario.Core.Installers.Resolver;
using Scenario.Core.Installers.Systems;
using Zenject;

namespace Scenario.Utilities.Extensions
{
    public static class ContainerExtensions
    {
        public static ConcreteIdArgConditionCopyNonLazyBinder BindScenarioSystem<TSystem>(this DiContainer container, 
            SystemResolver resolver) where TSystem : class
        {
            var binder = container.BindInterfacesAndSelfTo<TSystem>().AsSingle();
            resolver.AddPromiseResolve<TSystem>();
            return binder;
        }
    }
}