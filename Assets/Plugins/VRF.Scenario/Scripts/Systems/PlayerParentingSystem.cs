using System.Collections.Generic;
using Scenario.Core.Systems;
using Scenario.Utilities;
using UnityEngine;
using VRF.Players.Core;
using VRF.Scenario.Components.Actions;
using Zenject;

namespace VRF.Scenario.Systems
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class PlayerParentingSystem : BaseScenarioSystem
    {
        private readonly PlayersContainer playersContainer;
        private Transform defaultParent, player;

        public PlayerParentingSystem(SignalBus bus, PlayersContainer playersContainer) : base(bus)
        {
            this.playersContainer = playersContainer;
            
            playersContainer.PlayerChanged += PlayerChanged;
            
            bus.Subscribe<SetPlayerParent>(SetPlayerParent);
            bus.Subscribe<ResetPlayerParent>(ResetPlayerParent);
        }

        private void PlayerChanged()
        {
            player = playersContainer.CurrentValue.View.transform;
            defaultParent = player.parent;
        }

        private void SetPlayerParent(SetPlayerParent component)
        {
            if (AssertLog.NotNull<SetPlayerParent>(component.Parent, nameof(component.Parent))) return;
            if (AssertLog.NotNull<PlayerParentingSystem>(player, nameof(player))) return;
            
            player.SetParent(component.Parent, component.WorldPositionStays); 
        }
        
        private void ResetPlayerParent(ResetPlayerParent component)
        {
            if (AssertLog.NotNull<PlayerParentingSystem>(player, nameof(player))) return;
            
            player.SetParent(defaultParent, component.WorldPositionStays); 
        }
    }
}