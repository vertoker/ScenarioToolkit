using Scenario.Editor.Utilities.Providers;
using Scenario.Editor.Windows.GraphEditor;
using Scenario.Editor.Windows.GraphEditor.GraphViews;
using UnityEngine.UIElements;

namespace Scenario.Editor.Windows.ElementEditor.VisualElements
{
    public class LinkElement : BaseScenarioElementEditor
    {
        private readonly TextField nodeHashText;
        private readonly Button selectPrev;
        private readonly Button selectNext;
        
        protected readonly ScenarioLinkView LinkView;
        private readonly GraphEditorWindow graphEditor;
        protected readonly TemplateContainer Root;
        
        public LinkElement(ScenarioLinkView linkView, GraphEditorWindow graphEditor)
        {
            LinkView = linkView;
            this.graphEditor = graphEditor;

            Root = UIProvider.GetUxmlTree("ElementEditor/LinkEditor").Instantiate();
            Add(Root);
            
            nodeHashText = Root.Q<TextField>("hash");
            
            selectPrev = new Button(SelectPrev)
            {
                text = "Select prev",
                style =
                {
                    width = new StyleLength(new Length(50, LengthUnit.Percent)),
                },
            };
            selectNext = new Button(SelectNext)
            {
                text = "Select next",
                style =
                {
                    width = new StyleLength(new Length(50, LengthUnit.Percent)),
                },
            };
            var horizontalStack = new VisualElement
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                },
            };
            
            horizontalStack.Add(selectPrev);
            horizontalStack.Add(selectNext);
            Root.Add(horizontalStack);
        }

        private void SelectPrev()
        {
            var nodeHash = LinkView.ScenarioLink.From.Hash;
            var nodeView = graphEditor.GraphController.NodeViews[nodeHash];
            graphEditor.GraphView.SelectOnly(nodeView);
        }
        private void SelectNext()
        {
            var nodeHash = LinkView.ScenarioLink.To.Hash;
            var nodeView = graphEditor.GraphController.NodeViews[nodeHash];
            graphEditor.GraphView.SelectOnly(nodeView);
        }
        
        public override void RedrawElements()
        {
            nodeHashText.SetValueWithoutNotify(LinkView.ScenarioLink.Hash.ToString());
        }
        private void OnDataUpdated()
        {
            RedrawElements();
            InvokeDataUpdated();
        }
    }
}