using Scenario.Base.Components.Actions;
using Scenario.Core.Systems;
using Zenject;

namespace Scenario.Systems
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class CoreScenarioSystem : BaseScenarioSystem
    {
        public CoreScenarioSystem(SignalBus bus) : base(bus)
        {
            bus.Subscribe<StopScenarioContext>(StopScenarioContext);
        }

        private void StopScenarioContext(StopScenarioContext component)
        {
            component.Player.Stop();
        }
    }
}