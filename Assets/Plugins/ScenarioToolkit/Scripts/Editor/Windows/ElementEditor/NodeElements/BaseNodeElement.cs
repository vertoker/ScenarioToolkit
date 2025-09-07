using System;
using Scenario.Core.Model;
using Scenario.Core.Model.Interfaces;
using Scenario.Editor.Utilities.Providers;
using Scenario.Editor.Windows.ElementEditor.VisualElements;
using Scenario.Editor.Windows.GraphEditor;
using Scenario.Editor.Windows.GraphEditor.GraphViews;
using Scenario.Utilities;
using UnityEngine.UIElements;

namespace Scenario.Editor.Windows.ElementEditor.NodeElements
{
    public class BaseNodeElement : BaseScenarioElementEditor
    {
        private readonly TextField nodeName;
        private readonly TextField nodeHash;
        
        protected readonly ScenarioNodeView NodeView;
        protected readonly GraphEditorWindow GraphEditor;
        protected readonly TemplateContainer Root;
        
        public BaseNodeElement(ScenarioNodeView nodeView, GraphEditorWindow graphEditor)
        {
            NodeView = nodeView;
            GraphEditor = graphEditor;

            Root = UxmlEditorProvider.instance.ElementNodeEditor.Instantiate();
            Add(Root);
            
            nodeName = Root.Q<TextField>("name");
            nodeHash = Root.Q<TextField>("hash");
            
            nodeName.RegisterCallback<ChangeEvent<string>>(UpdateName);
            nodeHash.AddManipulator(new ContextualMenuManipulator(ConstructHash));
        }

        public override void RedrawElements()
        {
            nodeName.SetValueWithoutNotify(NodeView.ScenarioNode.Name);
            nodeHash.SetValueWithoutNotify(NodeView.ScenarioNode.Hash.ToString());
        }

        protected void OnDataUpdated() => OnDataUpdated(true);
        protected virtual void OnDataUpdated(bool redrawElements)
        {
            if (redrawElements) RedrawElements();
            NodeView.UpdateVisuals();
            InvokeDataUpdated();
        }

        private void UpdateName(ChangeEvent<string> evt)
        {
            NodeView.ScenarioNode.Name = evt.newValue;
            
            OnDataUpdated();
        }
        
        private void ConstructHash(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction("Randomize Hash", RandomizeHash, DropdownMenuAction.AlwaysEnabled);
        }
        
        private void RandomizeHash(DropdownMenuAction act)
        {
            var oldHash = NodeView.ScenarioNode.Hash;
            var newHash = NodeView.ScenarioNode.RandomizeHash();
            
            GraphEditor.GraphController.UpdateNodeHash(NodeView, oldHash, newHash);
            
            RedrawElements();
            OnDataUpdated();
        }
        
        protected EnumField CreateActivationTypeEnum(IScenarioNodeFlow flowNode)
        {
            var enumField = new EnumField("ActivationType", flowNode.ActivationType);
            enumField.RegisterCallback<ChangeEvent<Enum>>(Callback);
            return enumField;
            
            void Callback(ChangeEvent<Enum> changeEvent)
            {
                flowNode.ActivationType = (ActivationType)changeEvent.newValue;
                OnDataUpdated(true);
            }
        }
    }
}