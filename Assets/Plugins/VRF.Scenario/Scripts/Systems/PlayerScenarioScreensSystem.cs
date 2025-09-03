using Scenario.Core;
using Scenario.Core.Systems;
using VRF.Scenario.Components.Actions;
using VRF.Scenario.Services;
using VRF.Scenario.States;
using Zenject;

namespace VRF.Scenario.Systems
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class PlayerScenarioScreensSystem : BaseScenarioStateSystem<PlayerScenarioScreensState>
    {
        private readonly PlayerScenarioScreensService playerScreens;

        public PlayerScenarioScreensSystem(PlayerScenarioScreensService playerScreens, SignalBus bus) : base(bus)
        {
            this.playerScreens = playerScreens;
            
            bus.Subscribe<SetBindInfoTipActivity>(SetBindInfoTipActivity);
        }

        protected override void ApplyState(PlayerScenarioScreensState state)
        {
            playerScreens.SetMenuBindActive(state.InfoTipActivity);
        }

        private void SetBindInfoTipActivity(SetBindInfoTipActivity component)
        {
            State.InfoTipActivity = component.Active;
                
            playerScreens.SetMenuBindActive(component.Active);
        }
    }
}