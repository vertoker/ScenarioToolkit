using Scenario.Base.Components.Actions;
using ScenarioToolkit.Core.Systems;
using ScenarioToolkit.Core.Systems.States;
using ScenarioToolkit.Library.States;
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
    public class HingeLimitsSystem : BaseScenarioStateSystem<HingeLimitsState>
    {
        public HingeLimitsSystem(SignalBus bus) : base(bus)
        {
            bus.Subscribe<SetHingeLimits>(SetHingeLimits);
        }

        protected override void ApplyState(HingeLimitsState state)
        {
            foreach (var (key, (defaultData, data)) in state.Limits)
            {
                key.limits = new JointLimits() { min = data.Min, max = data.Max };
            }
        }

        private void SetHingeLimits(SetHingeLimits component)
        {
            if (AssertLog.NotNull<SetHingeLimits>(component.HingeJoint, nameof(component.HingeJoint))) return;

            var limits = component.HingeJoint.limits;

            var dataDefault = new HingeLimitsState.Data(limits.min, limits.max);
            var dataCurrent = new HingeLimitsState.Data(component.MinAngle, component.MaxAngle);
            State.Limits.SetStateData(component.HingeJoint, dataDefault, dataCurrent);
            
            limits.min = component.MinAngle;
            limits.max = component.MaxAngle;
            component.HingeJoint.limits = limits;
        }
    }
}