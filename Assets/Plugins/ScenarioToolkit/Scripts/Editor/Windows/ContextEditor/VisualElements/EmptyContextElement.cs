using Scenario.Editor.Utilities.Providers;

namespace Scenario.Editor.Windows.ContextEditor.VisualElements
{
    public class EmptyContextElement : BaseContextElement
    {
        public EmptyContextElement() : base(UxmlEditorProvider.instance.ContextEmptyEditor, UssEditorProvider.instance.ContextEditor)
        {
            
        }
    }
}