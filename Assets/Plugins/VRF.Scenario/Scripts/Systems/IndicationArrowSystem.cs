using Scenario.Core;
using Scenario.Core.Systems;
using Scenario.Utilities;
using UnityEngine;
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
    public class IndicationArrowSystem : BaseScenarioAsyncStateSystem<IndicationArrowState>
    {
        public IndicationArrowSystem(SignalBus listener) : base(listener)
        {
            listener.Subscribe<SetArrowValue>(SetArrowValue);
            listener.Subscribe<AddArrowValue>(AddArrowValue);
            listener.Subscribe<SubtractArrowValue>(SubtractArrowValue);
        }

        protected override void ApplyState(IndicationArrowState state)
        {
            foreach (var (indicationArrow, data) in state.Arrows)
            {
                indicationArrow.ValueReached += OnArrowValueReached;
                
                indicationArrow.SetValue(Mathf.Lerp(data.StartValue, data.TargetValue, (float)(data.GetPassedTime() / data.Seconds)));
                var remainingTime = (float)data.GetRemainingTime();
                if(remainingTime > 0)
                    indicationArrow.SetValue(data.TargetValue, remainingTime);
            }
        }

        private void OnArrowValueReached(IndicationArrow arrow)
        {
            Bus.Fire(new ArrowValueReached
            {
                IndicationArrow = arrow
            });
        }

        private void SetArrowValue(SetArrowValue component)
        {
            var indicationArrow = component.IndicationArrow;
            var value = component.Value;
            var time = component.Time;

            if (AssertLog.NotNull<SetArrowValue>(indicationArrow, nameof(indicationArrow))) return;

            State.Arrows[indicationArrow] = new IndicationArrowState.Data(indicationArrow.CurrentValue, value, time);
            
            indicationArrow.SetValue(value, time);
            
        }

        private void AddArrowValue(AddArrowValue component)
        {
            var indicationArrow = component.IndicationArrow;
            var value = component.Value;
            var time = component.Time;

            if (AssertLog.NotNull<AddArrowValue>(indicationArrow, nameof(indicationArrow))) return;

            State.Arrows[indicationArrow] = new IndicationArrowState.Data(indicationArrow.CurrentValue, indicationArrow.CurrentValue + value, time);
            
            indicationArrow.AddValue(value, time);
        }

        private void SubtractArrowValue(SubtractArrowValue component)
        {
            var indicationArrow = component.IndicationArrow;
            var value = component.Value;
            var time = component.Time;

            if (AssertLog.NotNull<SubtractArrowValue>(indicationArrow, nameof(indicationArrow))) return;

            State.Arrows[indicationArrow] = new IndicationArrowState.Data(indicationArrow.CurrentValue, indicationArrow.CurrentValue - value, time);
            
            indicationArrow.SubtractValue(value, time);
        }
    }
}