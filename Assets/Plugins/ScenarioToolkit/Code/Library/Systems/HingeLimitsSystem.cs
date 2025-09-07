using Scenario.Base.Components.Actions;
using ScenarioToolkit.Core.Systems;
using ScenarioToolkit.Shared;
using UnityEngine;
using Zenject;

namespace ScenarioToolkit.Library.Systems
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    /// <summary>
    /// State система для установки лимитов для HingeJoint
    /// </summary>
    public class HingeLimitsSystem : BaseScenarioSystem
    {
        public HingeLimitsSystem(SignalBus bus) : base(bus)
        {
            bus.Subscribe<SetHingeLimits>(SetHingeLimits);
        }
        
        private void SetHingeLimits(SetHingeLimits component)
        {
            if (AssertLog.NotNull<SetHingeLimits>(component.HingeJoint, nameof(component.HingeJoint))) return;

            var limits = component.HingeJoint.limits;
            
            limits.min = component.MinAngle;
            limits.max = component.MaxAngle;
            component.HingeJoint.limits = limits;
        }
    }
}