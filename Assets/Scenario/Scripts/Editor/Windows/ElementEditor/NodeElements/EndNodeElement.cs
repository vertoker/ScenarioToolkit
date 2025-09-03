using Scenario.Core.Model.Interfaces;
using Scenario.Editor.Windows.GraphEditor;
using Scenario.Editor.Windows.GraphEditor.GraphViews;
using UnityEngine.UIElements;

namespace Scenario.Editor.Windows.ElementEditor.NodeElements
{
    public class EndNodeElement : BaseNodeElement
    {
        private readonly IEndNode endNode;

        public EndNodeElement(ScenarioNodeView nodeView, GraphEditorWindow graphEditor)
            : base(nodeView, graphEditor)
        {
            endNode = (IEndNode)nodeView.ScenarioNode;
            
            var enumField = CreateActivationTypeEnum(endNode);
            Root.Add(enumField);
            
            var field = new Toggle(nameof(endNode.InstantEnd))
            {
                value = endNode.InstantEnd,
            };
            Root.Add(field);
            
            field.RegisterValueChangedCallback(ObjectChanged);
        }

        private void ObjectChanged(ChangeEvent<bool> evt)
        {
            endNode.InstantEnd = evt.newValue;
            OnDataUpdated();
        }
    }
}