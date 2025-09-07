using ScenarioToolkit.Shared;
using Zenject;

namespace ScenarioToolkit.Core.Installers
{
    public class ScenarioSignalBusInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            if (!Container.HasBinding<SignalBus>())
                SignalBusInstaller.Install(Container);

            foreach (var type in ComponentsReflection.AllComponentTypes)
                Container.DeclareSignal(type);
        }
    }
}