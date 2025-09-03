using System.Collections.Generic;
using Scenario.Core.Systems;
using VRF.Scenario.Components.Conditions.WorldButtons;
using VRF.VRBehaviours;
using Zenject;

namespace VRF.Scenario.Systems
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class WorldButtonSystem : BaseScenarioSystem
    {
        public WorldButtonSystem(SignalBus bus, IEnumerable<WorldButton> worldButtons) : base(bus)
        {
            foreach (var worldButton in worldButtons)
            {
                worldButton.ButtonPressed.AddListener(() => bus.Fire(new WorldButtonPressed() { WorldButton = worldButton }));
                worldButton.ButtonReleased.AddListener(() => bus.Fire(new WorldButtonReleased() { WorldButton = worldButton }));
            }
        }
    }
}