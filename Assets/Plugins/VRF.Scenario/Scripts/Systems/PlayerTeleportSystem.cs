using Scenario.Core;
using Scenario.Core.Systems;
using Scenario.Utilities;
using VRF.BNG_Framework.Scripts.Core;
using VRF.Components.Players;
using VRF.Components.Players.Views.Player;
using VRF.Players.Core;
using VRF.Scenario.Components.Actions;
using Zenject;

namespace VRF.Scenario.Systems
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class PlayerTeleportSystem : BaseScenarioSystem
    {
        private readonly Players.Core.PlayersContainer playersContainer;
        
        public PlayerTeleportSystem(SignalBus bus, Players.Core.PlayersContainer playersContainer) : base(bus)
        {
            this.playersContainer = playersContainer;
            bus.Subscribe<TeleportPlayer>(TeleportPlayer);
        }

        private void TeleportPlayer(TeleportPlayer component)
        {
            if (AssertLog.NotNull<TeleportPlayer>(component.Target, nameof(component.Target))) return;
            
            playersContainer.CurrentValue.View.PlayerTeleport.TeleportPlayerToTransform(component.Target);
        }
    }
}