using Scenario.Core.Systems;
using Scenario.Utilities;
using UnityEngine.Rendering;
using VRF.Players.Core;
using VRF.Scenario.Components.Actions;
using Zenject;

namespace VRF.Scenario.Systems
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class PlayerVolumeSystem : BaseScenarioSystem
    {
        private readonly PlayersContainer playersContainer;
        private Volume volume;

        public PlayerVolumeSystem(SignalBus bus, PlayersContainer playersContainer) : base(bus)
        {
            this.playersContainer = playersContainer;
            
            playersContainer.PlayerChanged += PlayerChanged;
            
            bus.Subscribe<SetPlayerVolume>(SetPlayerVolume);
            bus.Subscribe<ResetPlayerVolume>(ResetPlayerVolume);
        }

        private void PlayerChanged()
        {
            var prevVolume = volume;
            volume = playersContainer.CurrentValue.View.Volume;
            
            if (prevVolume && volume)
            {
                volume.sharedProfile = prevVolume.sharedProfile;
                volume.enabled = prevVolume.enabled;
                volume.gameObject.SetActive(prevVolume.gameObject.activeSelf);
                volume.weight = prevVolume.weight;
            }
        }

        private void SetPlayerVolume(SetPlayerVolume component)
        {
            if (AssertLog.NotNull<SetAppearance>(component.Profile, nameof(component.Profile))) return;
            if (AssertLog.NotNull<PlayerParentingSystem>(volume, nameof(volume))) return;
            
            volume.sharedProfile = component.Profile;
            volume.enabled = true;
            volume.gameObject.SetActive(true);
            volume.weight = 1;
        }
        private void ResetPlayerVolume(ResetPlayerVolume component)
        {
            if (AssertLog.NotNull<PlayerParentingSystem>(volume, nameof(volume))) return;

            volume.sharedProfile = null;
            volume.weight = 0;
        }
    }
}