using System;
using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Editor.Windows.ElementEditor.NodeElements;
using ScenarioToolkit.Editor.Windows.ElementEditor.NodeElements.Port;
using ScenarioToolkit.Editor.Windows.ElementEditor.VisualElements;
using ScenarioToolkit.Editor.Windows.GraphEditor;
using ScenarioToolkit.Editor.Windows.GraphEditor.GraphViews;

namespace ScenarioToolkit.Editor.Windows.ElementEditor
{
    public class ElementEditorWindow : BaseScenarioWindow
    {
        public event Action DataUpdated;
        private BaseScenarioElementEditor currentElement;

        private GraphEditorWindow GraphEditor { get; set; }
        public NodeOverridesController OverridesController { get; } = new();

        public void Construct(GraphEditorWindow graphEditor, IScenarioContext context)
        {
            GraphEditor = graphEditor;
            OverridesController.BindScenario(context);
        }
        public void BindNode(ScenarioNodeView nodeView)
        {
            UnbindElement();
            ShowTab();

            var nodeElement = nodeView.ScenarioNode switch
            {
                IActionNode => new ComponentsNodeElement<IScenarioAction>(nodeView, GraphEditor, OverridesController),
                IConditionNode => new ComponentsNodeElement<IScenarioCondition>(nodeView, GraphEditor, OverridesController),
                ISubgraphNode => new SubgraphNodeElement(nodeView, GraphEditor),
                
                IPortInNode => new PortInNodeElement(nodeView, GraphEditor),
                IPortOutNode => new PortOutNodeElement(nodeView, GraphEditor),
                INoteNode => new NoteNodeElement(nodeView, GraphEditor),
                IEndNode => new EndNodeElement(nodeView, GraphEditor),
                IStartNode => new StartNodeElement(nodeView, GraphEditor),
                
                //_ => throw new NotImplementedException("UI Element for node is not founded")
                _ => new BaseNodeElement(nodeView, GraphEditor)
            };
            
            BindElement(nodeElement);
        }
        public void BindLink(ScenarioLinkView linkView)
        {
            UnbindElement();
            ShowTab();
            
            var linkElement = new LinkElement(linkView, GraphEditor);
            
            BindElement(linkElement);
        }
        public void BindGroup(ScenarioGroupView groupView)
        {
            UnbindElement();
            ShowTab();

            var groupElement = new GroupElement(groupView, GraphEditor);
            
            BindElement(groupElement);
        }
        
        private void BindElement(BaseScenarioElementEditor newElement)
        {
            currentElement = newElement;
            currentElement.RedrawElements();
            currentElement.DataUpdated += OnDataUpdate;
            rootVisualElement.Add(newElement);
        }
        public void UnbindElement()
        {
            if (currentElement != null)
                currentElement.DataUpdated -= OnDataUpdate;
            currentElement = null;
            rootVisualElement.Clear();
            OverridesController.Save();
        }

        private void OnDataUpdate()
        {
            DataUpdated?.Invoke();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            SWEContext.SetElement(this);
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            SWEContext.SetElement(null);
        }
    }
}