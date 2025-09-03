using System.Collections.Generic;
using Scenario.Core.Systems;
using VRF.Scenario.Components.Conditions.ToggleSwitches;
using VRF.VRBehaviours;
using Zenject;

namespace VRF.Scenario.Systems
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class ToggleSwitchSystem : BaseScenarioSystem
    {
        public ToggleSwitchSystem(SignalBus bus, IEnumerable<ToggleSwitch> toggleSwitches) : base(bus)
        {
            foreach (var toggleSwitch in toggleSwitches)
            {
                toggleSwitch.On.AddListener(() => bus.Fire(new ToggleSwitchOn() { ToggleSwitch = toggleSwitch }));
                toggleSwitch.Off.AddListener(() => bus.Fire(new ToggleSwitchOff() { ToggleSwitch = toggleSwitch }));
            }
        }
    }
}