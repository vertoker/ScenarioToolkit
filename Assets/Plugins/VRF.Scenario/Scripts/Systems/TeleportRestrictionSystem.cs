using Scenario.Core.Systems;
using VRF.BNG_Framework.Scripts.Core;
using VRF.Players.Core;
using VRF.Players.Models;
using VRF.Scenario.Components.Actions;
using Zenject;

namespace VRF.Scenario.Systems
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class TeleportRestrictionSystem : BaseScenarioSystem
    {
        private readonly PlayersContainer playersContainer;
        private PlayerTeleport playerTeleport;

        public TeleportRestrictionSystem(SignalBus listener, PlayersContainer playersContainer) : base(listener)
        {
            this.playersContainer = playersContainer;
            
            playersContainer.PlayerChanged += PlayerChanged;
        }

        private void PlayerChanged()
        {
            Unsubscribe();
            UpdateTeleport();
            Subscribe();
        }

        private void UpdateTeleport()
        {
            playerTeleport = playersContainer.CurrentValue.View.PlayerTeleport;
        }

        private void Subscribe()
        {
            if (!playerTeleport) return;
            
            Bus.Subscribe<ForbidTeleport>(playerTeleport.DisableTeleportation);
            Bus.Subscribe<AllowTeleport>(playerTeleport.EnableTeleportation);
        }
        private void Unsubscribe()
        {
            if (!playerTeleport) return;
            
            Bus.Unsubscribe<ForbidTeleport>(playerTeleport.DisableTeleportation);
            Bus.Unsubscribe<AllowTeleport>(playerTeleport.EnableTeleportation);
        }
    }
}