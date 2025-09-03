using System.Collections.Generic;
using Scenario.Core;
using Scenario.Core.Systems;
using VRF.BNG_Framework.Scripts.Core;
using VRF.Scenario.Components.Conditions;
using Zenject;

namespace VRF.Scenario.Systems
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class GrabbingSystem : BaseScenarioSystem
    {
        public GrabbingSystem(SignalBus listener, IEnumerable<Grabbable> grabbables) : base(listener)
        {
            foreach (var grabbable in grabbables)
                grabbable.OnGrabbed += () => OnGrabbed(grabbable);
        }

        private void OnGrabbed(Grabbable grabbable)
        {
            Bus.Fire(new GrabbableGrabbed
            {
                Value = grabbable
            });
        }
    }
}