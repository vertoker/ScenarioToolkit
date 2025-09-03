using System.Collections.Generic;
using Scenario.Core;
using Scenario.Core.Systems;
using VRF.Scenario.Components.Conditions;
using VRF.VRBehaviours.Keys;
using Zenject;

namespace VRF.Scenario.Systems
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class KeysSystem : BaseScenarioSystem
    {
        public KeysSystem(SignalBus bus, IEnumerable<Keyhole> keyholes) : base(bus)
        {
            foreach (var keyhole in keyholes)
            {
                keyhole.Opened.AddListener(() => bus.Fire(new KeyholeOpened() { Keyhole = keyhole }));
                keyhole.Closed.AddListener(() => bus.Fire(new KeyholeClosed() { Keyhole = keyhole }));
            }
        }
    }
}