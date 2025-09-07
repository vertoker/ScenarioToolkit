using Scenario.Editor.Utilities.Providers;
using Scenario.Editor.Windows.GraphEditor;
using Scenario.Editor.Windows.GraphEditor.GraphViews;
using Scenario.Utilities;
using UnityEngine.UIElements;

namespace Scenario.Editor.Windows.ElementEditor.VisualElements
{
    public class GroupElement : BaseScenarioElementEditor
    {
        private readonly TextField nodeName;
        private readonly TextField nodeHash;
        
        private readonly TextElement nodesCountText;
        
        private readonly ScenarioGroupView groupView;
        private readonly GraphEditorWindow graphEditor;

        public GroupElement(ScenarioGroupView groupView, GraphEditorWindow graphEditor)
        {
            this.groupView = groupView;
            this.graphEditor = graphEditor;

            var root = UxmlEditorProvider.instance.ElementGroupEditor.Instantiate();
            Add(root);
            
            nodeName = root.Q<TextField>("name");
            nodeHash = root.Q<TextField>("hash");
            
            nodeName.RegisterCallback<ChangeEvent<string>>(UpdateName);
            nodeHash.AddManipulator(new ContextualMenuManipulator(ConstructHash));

            var nodesCount = graphEditor.GraphController.GetGroupElements(groupView).Count;
            nodesCountText = new TextElement
            {
                // TODO классная разметка
                text = $"\n Nodes count: {nodesCount}",
            };
            Add(nodesCountText);
        }

        public override void RedrawElements()
        {
            nodeName.SetValueWithoutNotify(groupView.Name);
            nodeHash.SetValueWithoutNotify(groupView.Hash.ToString());
        }
        private void OnDataUpdated()
        {
            RedrawElements();
            InvokeDataUpdated();
        }
        
        private void UpdateName(ChangeEvent<string> evt)
        {
            groupView.Name = evt.newValue;
            OnDataUpdated();
        }
        
        private void ConstructHash(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction("Randomize Hash", RandomizeHash, DropdownMenuAction.AlwaysEnabled);
        }
        
        private void RandomizeHash(DropdownMenuAction act)
        {
            groupView.Hash = CryptoUtility.RandomizeHash();
            
            RedrawElements();
            OnDataUpdated();
        }
    }
}