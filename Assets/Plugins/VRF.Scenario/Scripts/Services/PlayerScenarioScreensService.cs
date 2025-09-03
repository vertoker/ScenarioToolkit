using System;
using Scenario.Core.Installers.Systems;
using SimpleUI.Anim;
using SimpleUI.Core;
using SimpleUI.Scriptables.Core;
using UnityEngine;
using UnityEngine.InputSystem;
using VRF.Components.Players;
using VRF.Components.Players.Views.Player;
using VRF.Players.Core;
using VRF.Players.Models;
using VRF.Scenario.Components.Actions;
using VRF.Scenario.Components.Conditions;
using VRF.Scenario.Systems;
using VRF.Scenario.UI.Game;
using VRF.Scenario.UI.Game.InfoTip;
using Zenject;

namespace VRF.Scenario.Services
{
    public class PlayerScenarioScreensService : IDisposable//, IInitializable
    {
        public ScreensManager InfoUI { get; private set; }
        public ScreensManager TimerUI { get; private set; }
        public bool Initialized { get; }

        private readonly PlayersContainer playersContainer;
        private readonly DiContainer diContainer;
        private SchemeVR schemeVR;
        private SchemeWASD schemeWASD;
        private readonly ExamSystem examSystem;
        private readonly ConstructEntry storages;
        private readonly IDebugParam source;

        public struct ConstructEntry
        {
            public ScreenStorage Info;
            public ScreenStorage Timer;
        }

        public PlayerScenarioScreensService(PlayersContainer playersContainer, DiContainer diContainer,
            ExamSystem examSystem,
            ConstructEntry storages, IDebugParam source)
        {
            Initialized = true;

            this.playersContainer = playersContainer;
            this.diContainer = diContainer;
            this.examSystem = examSystem;
            this.storages = storages;
            this.source = source;

            UpdateControlScheme();
            UpdateUIFromPlayerView();
            
            playersContainer.PlayerChanged += PlayerChanged;
            
            // фикс бага с порядком установки систем
            Initialize();
        }

        public void Initialize()
        {
            if (!Initialized) return;

            examSystem.ExamStarted += ExamStarted;
            examSystem.ExamUpdated += ExamUpdate;
            examSystem.ExamStopped += ExamEnded;

            EnableMenuBind();
        }

        public void Dispose()
        {
            if (!Initialized) return;

            examSystem.ExamStarted -= ExamStarted;
            examSystem.ExamUpdated -= ExamUpdate;
            examSystem.ExamStopped -= ExamEnded;

            DisableMenuBind();
        }

        private void PlayerChanged()
        {
            var prevInfoOpened = InfoUI?.IsOpened<InfoTipScreen>() ?? false;
            var prevTimerOpened = TimerUI?.IsOpened<TimerScreen>() ?? false;

            UpdateControlScheme();
            UpdateUIFromPlayerView();

            if (InfoUI != null)
            {
                var infoOpened = InfoUI.IsOpened<InfoTipScreen>();
                
                if (prevInfoOpened && !infoOpened)
                {
                    InfoUI.Open<InfoTipScreen>();
                }
                else if(!prevInfoOpened && infoOpened)
                {
                    InfoUI.Close<InfoTipScreen>();
                }
            }

            if (TimerUI != null)
            {
                var timerOpened = TimerUI.IsOpened<TimerScreen>();

                if (prevTimerOpened && !timerOpened)
                {
                    TimerUI.Open<TimerScreen>();
                }
                else if (!prevTimerOpened && timerOpened)
                {
                    TimerUI.Close<TimerScreen>();
                }
            }
        }

        private void UpdateUIFromPlayerView()
        {
            var playerView = playersContainer.CurrentValue.View;
            
            if (playerView)
            {
                if (playerView.InfoUI)
                {
                    InfoUI = playerView.InfoUI.Manager;
                    playerView.InfoUI.Storage = storages.Info;
                }
                else InfoUI = null;
                
                if (playerView.TimerUI)
                {
                    TimerUI = playerView.TimerUI.Manager;
                    playerView.TimerUI.Storage = storages.Timer;
                }
                else TimerUI = null;
            }
        }

        private void UpdateControlScheme()
        {
            var playerView = playersContainer.CurrentValue.View;

            if (playerView is PlayerVRView)
            {
                schemeVR = diContainer.Resolve<SchemeVR>();
            }
            else if (playerView is PlayerWASDView)
            {
                schemeWASD = diContainer.Resolve<SchemeWASD>();
            }

            if (menuBindActive)
            {
                DisableMenuBind();
                EnableMenuBind();
            }
        }

        #region Exam Timer

        private TimerScreen screen;

        private void ExamStarted(StartExam startExam)
        {
            screen = TimerUI.Find<TimerScreen>();
            TimerUI.Open<TimerScreen>();
        }

        private void ExamUpdate(TimerUpdate component)
        {
            screen.TextComponent?.SetText(component.GetTimeText());
        }

        private void ExamEnded(bool examPassed)
        {
            TimerUI.Close<TimerScreen>();
        }

        #endregion

        #region Menu Bind

        public void SetMenuBindActive(bool active)
        {
            if (active) EnableMenuBind();
            else DisableMenuBind();
        }

        private bool menuBindActive;

        private void EnableMenuBind()
        {
            if (menuBindActive) return;
            menuBindActive = true;

            if (schemeVR != null)
            {
                schemeVR.XRRightHand.Menu.performed += MenuPerformed;
                schemeVR.XRRightHand.Menu.Enable();
            }

            if (schemeWASD != null)
            {
                schemeWASD.UI.Info.performed += MenuPerformed;
                schemeWASD.UI.Info.Enable();
            }

            if (source.Debug)
                Debug.Log(nameof(EnableMenuBind));
        }

        private void DisableMenuBind()
        {
            if (!menuBindActive) return;
            menuBindActive = false;

            if (InfoUI != null && InfoUI.IsOpened<InfoTipScreen>())
                InfoUI.Close<InfoTipScreen>(AnimParameters.NoAnim);

            if (schemeVR != null)
            {
                schemeVR.XRRightHand.Menu.Disable();
                schemeVR.XRRightHand.Menu.performed -= MenuPerformed;
            }

            if (schemeWASD != null)
            {
                schemeWASD.UI.Info.Disable();
                schemeWASD.UI.Info.performed -= MenuPerformed;
            }

            if (source.Debug)
                Debug.Log(nameof(DisableMenuBind));
        }

        private void MenuPerformed(InputAction.CallbackContext ctx)
        {
            if (InfoUI.IsOpened<InfoTipScreen>())
                InfoUI.Close<InfoTipScreen>();
            else InfoUI.Open<InfoTipScreen>();
        }

        #endregion
    }
}