using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Editor.Windows.GraphEditor;
using ScenarioToolkit.Editor.Windows.GraphEditor.GraphViews;
using UnityEngine;

namespace ScenarioToolkit.Editor.Windows.ElementEditor.NodeElements.Port
{
    public class PortInNodeElement : BasePortNodeElement<IPortInNode, IPortOutNode>
    {
        public PortInNodeElement(ScenarioNodeView nodeView, GraphEditorWindow graphEditor)
            : base(nodeView, graphEditor)
        {
            var node = (IPortInNode)nodeView.ScenarioNode;
            var enumField = CreateActivationTypeEnum(node);
            Root.Add(enumField);
        }
        
        protected override IPortOutNode LinkedNode() => PortNode.OutputNode;
        protected override Vector2 GetOffsetLinkedSpawn() => new(100f, 0f);
        protected override IScenarioLinkFlow LinkNodes(ScenarioNodeView editorNode, ScenarioNodeView otherEditorNode)
        {
            var inNode = (IPortInNode)editorNode.ScenarioNode;
            var outNode = (IPortOutNode)otherEditorNode.ScenarioNode;
            inNode.OutputNode = outNode;
            outNode.InputNode = inNode;
            
            var link = IScenarioLinkFlow.CreateNew();
            link.From = inNode; link.To = outNode;
            return link;
        }
    }
    
    public class PortOutNodeElement : BasePortNodeElement<IPortOutNode, IPortInNode>
    {
        public PortOutNodeElement(ScenarioNodeView nodeView, GraphEditorWindow graphEditor)
            : base(nodeView, graphEditor) { }
        
        protected override IPortInNode LinkedNode() => PortNode.InputNode;
        protected override Vector2 GetOffsetLinkedSpawn() => new(-100f, 0f);
        protected override IScenarioLinkFlow LinkNodes(ScenarioNodeView editorNode, ScenarioNodeView otherEditorNode)
        {
            var outNode = (IPortOutNode)editorNode.ScenarioNode;
            var inNode = (IPortInNode)otherEditorNode.ScenarioNode;
            inNode.OutputNode = outNode;
            outNode.InputNode = inNode;

            var link = IScenarioLinkFlow.CreateNew();
            link.From = inNode; link.To = outNode;
            return link;
        }
    }
}