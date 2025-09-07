using System.Collections.Generic;
using ScenarioToolkit.Core.Systems.States;
using TMPro;

namespace ScenarioToolkit.Library.States
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