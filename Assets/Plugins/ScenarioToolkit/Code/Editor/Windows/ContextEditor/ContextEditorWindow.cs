using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Editor.Windows.ContextEditor.VisualElements;
using ScenarioToolkit.Editor.Windows.GraphEditor;

namespace ScenarioToolkit.Editor.Windows.ContextEditor
{
    public class ContextEditorWindow : BaseScenarioWindow
    {
        private GraphEditorWindow GraphEditor { get; set; }
        public IScenarioModel Model { get; private set; }
        private BaseContextElement contextElement;
        
        public void Construct(GraphEditorWindow graphEditor)
        {
            GraphEditor = graphEditor;
        }
        public void BindScenario(IScenarioModel model)
        {
            Reset();
            Model = model;
            contextElement = new ActiveContextElement(Model, GraphEditor);
            contextElement.DataUpdated += SetDirtyScenario;
            rootVisualElement.Add(contextElement.Root);
            
            ShowTab();
        }
        public void BindEmptyScenario()
        {
            Reset();
            Model = null;
            contextElement = new EmptyContextElement();
            rootVisualElement.Add(contextElement.Root);
        }
        
        protected override void OnEnable()
        {
            base.OnEnable();
            SWEContext.SetContext(this);
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            SWEContext.SetContext(null);
        }
        private void Reset()
        {
            contextElement?.Reset();
            rootVisualElement.Clear();
            SaveWindow();
        }
    }
}