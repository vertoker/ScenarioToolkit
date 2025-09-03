using System.Collections.Generic;
using Scenario.Core.Systems.States;
using SimpleUI.Core;

namespace VRF.Scenario.States
{
    public class ScreensState : IState
    {
        public Dictionary<ScreenBase, bool> ScreensActivity = new();
        
        public void Clear()
        {
            ScreensActivity.Clear();
        }
    }
}