using System.Collections.Generic;
using Scenario.Core.Systems.States;
using VRF.Dialog;

namespace VRF.Scenario.States
{
    public class DialogState : IState
    {
        public HashSet<DialogLineConfig> Dialogs = new();
        
        public void Clear()
        {
            Dialogs.Clear();
        }
    }
}