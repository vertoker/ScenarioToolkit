using System;
using Scenario.Editor.Utilities.Providers;
using UnityEngine.UIElements;

namespace Scenario.Editor.Windows.ContextEditor.VisualElements
{
    public abstract class BaseContextElement : VisualElement
    {
        public event Action DataUpdated;
        
        public TemplateContainer Root { get; private set; }
        
        protected BaseContextElement()
        {
            Root = UIProvider.GetUxmlTree(UxmlAssetName).Instantiate();
            Root.styleSheets.Add(UIProvider.GetUssSheet(StylesAssetName));
            Add(Root);
        }
        
        protected abstract string UxmlAssetName { get; }
        protected abstract string StylesAssetName { get; }

        public virtual void Reset()
        {
            
        }

        protected void OnDataUpdated()
        {
            DataUpdated?.Invoke();
        }
    }
}