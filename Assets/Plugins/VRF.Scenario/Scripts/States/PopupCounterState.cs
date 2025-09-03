using System.Collections.Generic;
using Scenario.Core.Systems.States;
using VRF.Scenario.UI.ScenarioGame;

namespace VRF.Scenario.States
{
    public class PopupCounterState : IState
    {
        public Dictionary<PopupScreen, int> ClickedPopupClickCounts = new();
        
        public void Clear()
        {
            ClickedPopupClickCounts.Clear();
        }
    }
}