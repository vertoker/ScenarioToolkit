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
    public class TransformStatesSystem : BaseScenarioStateSystem<TransformStatesState>
    {
        public TransformStatesSystem(SignalBus listener, IEnumerable<GOTransformStates> states) : base(listener)
        {
            foreach (var state in states)
            {
                var defaultState = state.State;
                state.StateChanged += (transformStates, i) =>
                {
                    State.States.SetStateData(state, defaultState, i);
                    
                    Bus.Fire(new TransformStateReached
                    {
                        States = transformStates,
                        State = i
                    });
                };
            }

            Bus.Subscribe<SetTransformState>(SetTransformState);
        }

        protected override void ApplyState(TransformStatesState state)
        {
            foreach (var (goTransformStates, (defaultState, currentState)) in state.States)
            {
                goTransformStates.SetStateImmediate(currentState, true);
            }
        }

        private void SetTransformState(SetTransformState component)
        {
            if (AssertLog.NotNull<SetTransformState>(component.States, nameof(component.States))) return;
                
            component.States.SetState(component.State);
        }
    }
}