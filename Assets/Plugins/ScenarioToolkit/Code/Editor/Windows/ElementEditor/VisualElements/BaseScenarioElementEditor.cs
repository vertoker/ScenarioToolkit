using System;
using UnityEngine.UIElements;

namespace ScenarioToolkit.Editor.Windows.ElementEditor.VisualElements
{
    public abstract class BaseScenarioElementEditor : VisualElement
    {
        public event Action DataUpdated;

        protected void InvokeDataUpdated()
        {
            DataUpdated?.Invoke();
        }
        
        public abstract void RedrawElements();
    }
}