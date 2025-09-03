using System;
using System.Collections.Generic;
using Scenario.Core;
using Scenario.Core.Systems;
using Scenario.Core.Systems.States;
using Scenario.Utilities;
using VRF.Scenario.Components.Actions;
using VRF.Scenario.Components.Conditions;
using VRF.Scenario.States;
using VRF.VRBehaviours;
using Zenject;

namespace VRF.Scenario.Systems
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class GrabbableLeverSystem : BaseScenarioStateSystem<GrabbableLeverState>
    {
        public GrabbableLeverSystem(SignalBus bus, IEnumerable<GrabbableLever> steeringWheels) : base(bus)
        {
            foreach (var wheel in steeringWheels)
                wheel.SnapDegreeReached += i =>
                {
                    Bus.Fire(new LeverSnapDegreeReached
                    {
                        GrabbableLever = wheel,
                        SnapDegreesIndex = i
                    });
                };

            bus.Subscribe<SetLeverPosition>(SetLeverPosition);
            bus.Subscribe<SetLeverLimits>(SetLeverLimits);
        }

        protected override void ApplyState(GrabbableLeverState state)
        {
            foreach (var (grabbableLever, (defaultData, data)) in state.Limits)
            {
                grabbableLever.MinAngle = data.MinAngle;
                grabbableLever.MaxAngle = data.MaxAngle;
            }

            foreach (var (grabbableLever, (defaultData, data)) in state.TargetAngleIndexes)
            {
                grabbableLever.SetTargetAngle(data);
            }
        }

        private void SetLeverLimits(SetLeverLimits component)
        {
            var lever = component.Lever;
            
            if (AssertLog.NotNull<SetLeverLimits>(lever, nameof(lever))) return;

            var bindDefault = new GrabbableLeverState.LimitsData(lever.MinAngle, lever.MaxAngle);
            var bindCurrent = new GrabbableLeverState.LimitsData(component.MinAngle, component.MaxAngle);
            State.Limits.SetStateData(lever, bindDefault, bindCurrent);
            
            lever.MaxAngle = component.MaxAngle;
            lever.MinAngle = component.MinAngle;
        }

        private void SetLeverPosition(SetLeverPosition component)
        {
            var lever = component.Lever;
            
            if (AssertLog.NotNull<SetLeverPosition>(lever, nameof(lever))) return;
            
            State.TargetAngleIndexes.SetStateData(lever, lever.startSnapAngleIndex, component.Index);
            
            component.Lever.SetTargetAngle(component.Index);
        }
    }
}