using Scenario.Utilities;
using Zenject;

namespace Scenario.Core.Installers
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