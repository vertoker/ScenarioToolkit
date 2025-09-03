using System;
using SimpleUI.Core;
using SimpleUI.Extensions;
using UnityEngine;
using VRF.UI.GameMenu.Screens;
using Zenject;

namespace VRF.UI.GameMenu
{
    public class GameMenuController : UIController<GameMenuView>, IInitializable, IDisposable
    {
        private readonly ScreenSettingsModel settingsModel;
        private readonly ExitScreen exitScreen;

        public GameMenuController([InjectOptional] ScreenSettingsModel settingsModel, GameMenuView view) : base(view)
        {
            this.settingsModel = settingsModel;
            if (settingsModel == null) return;
            
            var baseAnim = View.Screen.SpawnScreenAnim(settingsModel.AnimType);
            if (baseAnim)
            {
                baseAnim.AnimTime = settingsModel.AnimTime;
                baseAnim.EasingType = settingsModel.EasingType;
            }

            exitScreen = View.Manager.Find<ExitScreen>();
            
            SetActivity();
        }

        private void SetActivity()
        {
            if (settingsModel != null)
            {
                View.InventoryBtn.SetActive(settingsModel.ShowInventory);
                View.DialogBtn.SetActive(settingsModel.ShowDialog);
                View.ControlsBtn.SetActive(settingsModel.ShowControls);
                View.AboutBtn.SetActive(settingsModel.ShowAbout);
                View.SettingsBtn.SetActive(settingsModel.ShowSettings);
                
                View.ExitGameBtn.SetActive(settingsModel.ShowQuitApplication);
                View.ExitMenuBtn.SetActive(settingsModel.ShowQuitToScene);
                
                if (exitScreen)
                {
                    exitScreen.YesGameButton.SetActive(settingsModel.ShowQuitApplication);
                    exitScreen.YesMenuButton.SetActive(settingsModel.ShowQuitToScene);
                    exitScreen.SceneSwitch.Scene = settingsModel.ToQuitScene;
                }
            }
        }

        public void Initialize()
        {
            View.Screen.OpenStarted += OnOpen;
            View.Screen.CloseStarted += OnClose;
        }
        public void Dispose()
        {
            View.Screen.OpenStarted -= OnOpen;
            View.Screen.CloseStarted -= OnClose;
        }

        private void OnOpen(ScreenBase screen)
        {
            SetActivity();
            
            if (View.NotificationPlayer 
                && View.gameObject.activeInHierarchy
                && settingsModel is { UseOnEnable: true }
                && settingsModel.EnableSound)
                PlaySound(settingsModel.EnableSound); else StopSound();
        }

        private void OnClose(ScreenBase screen)
        {
            if (View.NotificationPlayer 
                && View.gameObject.activeInHierarchy
                && settingsModel is { UseOnDisable: true }
                && settingsModel.DisableSound)
                PlaySound(settingsModel.DisableSound); else StopSound();
        }
        
        private void StopSound()
        {
            View.NotificationPlayer.Stop();
            View.NotificationPlayer.clip = null;
        }
        private void PlaySound(AudioClip clip)
        {
            View.NotificationPlayer.clip = clip;
            View.NotificationPlayer.Play();
        }
    }
}