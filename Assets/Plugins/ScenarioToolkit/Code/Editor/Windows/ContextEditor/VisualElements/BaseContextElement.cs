using System;
using UnityEngine.UIElements;

namespace ScenarioToolkit.Editor.Windows.ContextEditor.VisualElements
{
    public abstract class BaseContextElement : VisualElement
    {
        public event Action DataUpdated;
        
        public TemplateContainer Root { get; private set; }
        
        protected BaseContextElement(VisualTreeAsset uxmlAsset, StyleSheet ussAsset)
        {
            Root = uxmlAsset.Instantiate();
            Root.styleSheets.Add(ussAsset);
            Add(Root);
        }
        
        public virtual void Reset()
        {
            
        }

        protected void OnDataUpdated()
        {
            DataUpdated?.Invoke();
        }
    }
}