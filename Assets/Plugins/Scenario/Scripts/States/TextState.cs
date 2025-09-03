using System;
using System.Collections.Generic;
using Scenario.Core.Systems.States;
using TMPro;

namespace Scenario.States
{
    public class TextState : IState
    {
        // (default, current)
        public Dictionary<TMP_Text, (string, string)> Texts = new();
        
        public void Clear()
        {
            Texts.Clear();
        }
    }
}