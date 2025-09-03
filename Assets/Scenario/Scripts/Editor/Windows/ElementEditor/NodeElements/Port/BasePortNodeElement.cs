using Scenario.Core.Model.Interfaces;
using Scenario.Core.Nodes;
using Scenario.Editor.Windows.GraphEditor;
using Scenario.Editor.Windows.GraphEditor.GraphViews;
using Scenario.Utilities;
using UnityEngine;
using UnityEngine.UIElements;

namespace Scenario.Editor.Windows.ElementEditor.NodeElements.Port
{
    public abstract class BasePortNodeElement<TPortNode, TOtherPortNode> : BaseNodeElement
        where TPortNode : IScenarioNode where TOtherPortNode : IScenarioNode
    {
        protected readonly TPortNode PortNode;
        private readonly ScenarioGraphView graphView;
        private readonly ScenarioGraphController graphController;
        
        private readonly TextField textField;
        private readonly Button button;
        private readonly Clickable selectLinkedPort;
        private readonly Clickable createLinkedPort;

        protected BasePortNodeElement(ScenarioNodeView nodeView, GraphEditorWindow graphEditor)
            : base(nodeView, graphEditor)
        {
            PortNode = (TPortNode)nodeView.ScenarioNode;
            graphView = graphEditor.GraphView;
            graphController = graphEditor.GraphController;

            textField = new TextField("Node") 
                { focusable = false };
            button = new Button();
            
            Root.Add(textField);
            Root.Add(button);

            selectLinkedPort = new Clickable(SelectLinkedPort);
            createLinkedPort = new Clickable(CreateLinkedPort);
            
            Update();
        }

        private void Update()
        {
            if (LinkedNode() != null)
            {
                textField.value = LinkedNode().Hash.ToString();
                button.text = "Select & Center linked port";
                button.clickable = selectLinkedPort;
            }
            else
            {
                textField.value = "null";
                button.text = "Create & Center linked port";
                button.clickable = createLinkedPort;
            }
        }

        private void SelectLinkedPort()
        {
            graphView.ClearSelection();
            var node = LinkedNode();
            var nodeView = graphController.NodeViews[node.Hash];
            graphView.AddToSelection(nodeView);
            graphView.Show(nodeView);
        }
        private void CreateLinkedPort()
        {
            var position = NodeView.GetCenterPosition() + GetOffsetLinkedSpawn();
            var content = NodesContent.GetContent<TOtherPortNode>();
            
            graphView.CreateEditorNode(position, content, out var otherEditorNode);
            var link = LinkNodes(NodeView, otherEditorNode);
            graphController.Graph.AddLink(link);
            
            NodeView.UpdateVisuals();
            otherEditorNode.UpdateVisuals();
            SelectLinkedPort();
        }
        
        protected abstract TOtherPortNode LinkedNode();
        protected abstract Vector2 GetOffsetLinkedSpawn();
        protected abstract IScenarioLinkFlow LinkNodes(ScenarioNodeView editorNode, ScenarioNodeView otherEditorNode);
    }
}