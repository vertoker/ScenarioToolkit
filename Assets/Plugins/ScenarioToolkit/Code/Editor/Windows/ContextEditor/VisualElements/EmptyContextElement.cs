using ScenarioToolkit.Editor.Utilities.Providers;

namespace ScenarioToolkit.Editor.Windows.ContextEditor.VisualElements
{
    public class EmptyContextElement : BaseContextElement
    {
        public EmptyContextElement() : base(UxmlEditorProvider.instance.ContextEmptyEditor, UssEditorProvider.instance.ContextEditor)
        {
            
        }
    }
}