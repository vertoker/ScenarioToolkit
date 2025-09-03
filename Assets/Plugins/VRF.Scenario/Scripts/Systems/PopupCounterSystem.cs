using System;
using System.Collections.Generic;
using Scenario.Core;
using Scenario.Core.Systems;
using VRF.Scenario.Components.Conditions;
using VRF.Scenario.States;
using VRF.Scenario.UI.ScenarioGame;
using Zenject;

namespace VRF.Scenario.Systems
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class PopupCounterSystem : BaseScenarioStateSystem<PopupCounterState>, IDisposable
    {
        private readonly Dictionary<PopupScreen, int> popupClickCounts = new();
        private readonly PopupScreen[] screens;
        
        public PopupCounterSystem(PopupScreen[] screens, SignalBus bus) : base(bus)
        {
            this.screens = screens;
            
            foreach (var screen in this.screens)
            {
                var buttons = screen.Buttons;
                popupClickCounts[screen] = buttons.Count;
                foreach (var button in buttons)
                    button.onClick.AddListener(() => OnPopupClicked(screen));
            }
        }

        protected override void ApplyState(PopupCounterState state)
        {
            foreach (var (popupScreen, value) in state.ClickedPopupClickCounts)
            {
                popupClickCounts[popupScreen] = value;
            }
        }

        public void Dispose()
        {
            foreach (var screen in screens)
            {
                foreach (var button in screen.Buttons)
                    button.onClick.RemoveAllListeners();
            }
        }

        private void OnPopupClicked(PopupScreen screen)
        {
            popupClickCounts[screen]--;
            
            State.ClickedPopupClickCounts[screen] = popupClickCounts[screen];
            
            if (popupClickCounts[screen] <= 0)
                Bus.Fire(new PopupClicked { Popup = screen });
        }
    }
}