using Scenario.Core.Systems;
using Scenario.Utilities;
using SimpleUI.Core;
using UnityEngine;
using VRF.Players.Core;
using VRF.Scenario.Components.Actions;
using VRF.Scenario.Models;
using Zenject;

namespace VRF.Scenario.Systems
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class PopupCreateSystem : BaseScenarioSystem
    {
        private readonly PlayersContainer playersContainer;
        private readonly ShowAtLookSettings defaultSettings;
        private Camera playerCamera;

        public PopupCreateSystem(SignalBus listener, PlayersContainer playersContainer, ShowAtLookSettings defaultSettings) : base(listener)
        {
            this.playersContainer = playersContainer;
            this.defaultSettings = defaultSettings;
            
            playersContainer.PlayerChanged += PlayerChanged;
            
            Bus.Subscribe<ShowScreenAtLook>(ShowScreenAtLook);
            Bus.Subscribe<ShowPopupScreenAtLook>(ShowPopupScreenAtLook);
        }

        private void PlayerChanged()
        {
            playerCamera = playersContainer.CurrentValue.View.Camera;
        }

        private void ShowScreenAtLook(ShowScreenAtLook component)
        {
            if (AssertLog.NotNull<ShowScreenAtLook>(component.Screen, nameof(component.Screen))) return;
            var settings = component.OverrideGlobalProperties ? component.GetSettings() : defaultSettings;
            ShowScreenAtLook(component.Screen, settings);
        }
        
        private void ShowPopupScreenAtLook(ShowPopupScreenAtLook component)
        {
            if (AssertLog.NotNull<ShowScreenAtLook>(component.Screen, nameof(component.Screen))) return;
            var settings = component.OverrideGlobalProperties ? component.GetSettings() : defaultSettings;
            
            //if (!string.IsNullOrEmpty(component.Text))
            component.Screen.TitleComponent?.SetText(component.Title);
            //if (!string.IsNullOrEmpty(component.Title))
            component.Screen.DescriptionComponent?.SetText(component.Text);
            //if (!string.IsNullOrEmpty(component.ButtonText))
            component.Screen.ButtonTextComponent?.SetText(component.ButtonText);
            
            ShowScreenAtLook(component.Screen, settings);
        }

        private void ShowScreenAtLook(ScreenBase screen, ShowAtLookSettings settings)
        {
            if(!playerCamera) return;
            
            var position = playerCamera.transform.position
                           + Quaternion.AngleAxis(settings.offsetAngle, Vector3.up)
                           * playerCamera.transform.forward * settings.distanceSpawn;
            var rotation = Quaternion.LookRotation(playerCamera.transform.forward);
            
            screen.transform.position = position;
            screen.transform.rotation = rotation;
            screen.Open();
        }
    }
}