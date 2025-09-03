namespace Scenario.Editor.Windows.ContextEditor.VisualElements
{
    public class EmptyContextElement : BaseContextElement
    {
        public EmptyContextElement()
        {
            
        }
        
        protected override string UxmlAssetName => "ContextEditor/EditorEmpty";
        protected override string StylesAssetName => "ContextEditor";
    }
}