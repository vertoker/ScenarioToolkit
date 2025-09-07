using ScenarioToolkit.Core.Installers.Resolver;
using Zenject;

namespace ScenarioToolkit.Shared.Extensions
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