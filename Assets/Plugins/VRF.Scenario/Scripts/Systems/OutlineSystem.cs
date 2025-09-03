using EPOOutline;
using Scenario.Core.Systems;
using VRF.Players.Core;
using VRF.Scenario.States;
using Zenject;

namespace VRF.Scenario.Systems
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class OutlineSystem : BaseScenarioStateSystem<OutlineState>
    {
        private readonly PlayersContainer playersContainer;
        private Outliner[] outliners;
    
        public OutlineSystem(SignalBus bus, PlayersContainer playersContainer) : base(bus)
        {
            this.playersContainer = playersContainer;
            
            playersContainer.PlayerChanged += PlayerChanged;
            
            bus.Subscribe<SetOutlinersActivity>(SetOutlinersActivity);
        }

        protected override void ApplyState(OutlineState state)
        {
            if(outliners == null) return;
            
            foreach (var outliner in outliners)
                outliner.enabled = state.OutlinersActivity;
        }

        private void PlayerChanged()
        {
            UpdateOutliners();
            ApplyState(State);
        }

        private void UpdateOutliners()
        {
            outliners = playersContainer.CurrentValue.View.Outliners;
        }

        private void SetOutlinersActivity(SetOutlinersActivity component)
        {
            State.OutlinersActivity = component.IsActive;

            if (outliners == null) return;

            foreach (var outliner in outliners)
            {
                outliner.enabled = component.IsActive;
                outliner.OutlineLayerMask = component.IsActive ? long.MaxValue : 0;
            }
        }
    }
}