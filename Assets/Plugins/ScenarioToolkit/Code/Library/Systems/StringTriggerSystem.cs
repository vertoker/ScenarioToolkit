using Scenario.Base.Components.Actions;
using Scenario.Base.Components.Conditions;
using ScenarioToolkit.Bus;
using ScenarioToolkit.Core.Systems;
using ScenarioToolkit.Shared;
using Zenject;

namespace ScenarioToolkit.Library.Systems
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class StringTriggerSystem : BaseScenarioSystem
    {
        public StringTriggerSystem(ScenarioComponentBus bus) : base(bus)
        {
            bus.Subscribe<TriggerString>(CallStringTriggered);
        }

        private void CallStringTriggered(TriggerString component)
        {
            if (AssertLog.NotWhiteSpace<TriggerString>(component.String, nameof(component.String))) return;
            
            Bus.Fire(new StringTriggered { String = component.String });
        }
    }
}