using Scenario.Core;
using Scenario.Core.Systems;
using Scenario.Core.Systems.States;
using Scenario.Utilities;
using VRF.Scenario.Components.Actions;
using VRF.Scenario.States;
using Zenject;

namespace VRF.Scenario.Systems
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class IsDroppableSystem : BaseScenarioStateSystem<IsDroppableState>
    {
        public IsDroppableSystem(SignalBus listener) : base(listener)
        {
            listener.Subscribe<SetIsDroppable>(ChangeDropFieldStatus);
        }

        protected override void ApplyState(IsDroppableState state)
        {
            foreach (var (grabbable, isDroppable) in state.Grabbables)
            {
                grabbable.CanBeDropped = isDroppable;
            }
        }

        private void ChangeDropFieldStatus(SetIsDroppable component)
        {
            if (AssertLog.NotNull<SetIsDroppable>(component.GrabbableObject, nameof(component.GrabbableObject))) return;
            
            State.Grabbables.SetStateActivity(component.GrabbableObject, component.GrabbableObject.CanBeDropped, component.IsDroppable);
            
            component.GrabbableObject.CanBeDropped = component.IsDroppable;
        }
    }
}