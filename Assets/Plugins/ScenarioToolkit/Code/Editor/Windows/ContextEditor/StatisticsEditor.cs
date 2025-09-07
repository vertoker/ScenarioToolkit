using Scenario.Core.Model.Interfaces;
using UnityEngine.UIElements;

namespace ScenarioToolkit.Editor.Windows.ContextEditor
{
    public class StatisticsEditor
    {
        private readonly IScenarioModel model;

        private readonly TextElement nodesCount;
        private readonly TextElement linksCount;
        private readonly TextElement groupsCount;
        private readonly TextElement unsavedNodesCount;
        private readonly TextElement unsavedLinksCount;
        private readonly TextElement unsavedGroupsCount;

        private int savedNodes;
        private int savedLinks;
        private int savedGroups;

        public StatisticsEditor(IScenarioModel model, VisualElement statisticsRoot)
        {
            this.model = model;
            
            nodesCount = new TextElement();
            statisticsRoot.Add(nodesCount);
            linksCount = new TextElement();
            statisticsRoot.Add(linksCount);
            groupsCount = new TextElement();
            statisticsRoot.Add(groupsCount);
            
            unsavedNodesCount = new TextElement();
            statisticsRoot.Add(unsavedNodesCount);
            unsavedLinksCount = new TextElement();
            statisticsRoot.Add(unsavedLinksCount);
            unsavedGroupsCount = new TextElement();
            statisticsRoot.Add(unsavedGroupsCount);
        }

        public void LoadUpdate()
        {
            savedNodes = model.Graph.NodesCount;
            savedLinks = model.Graph.LinksCount;
            savedGroups = model.EditorGraph.GroupsCount;
            Update();
        }
        public void Update()
        {
            //var currentNodes = graphEditor.GraphPresenter.NodeViewsCount;
            //var currentLinks = graphEditor.GraphPresenter.LinkViewsCount;
            //var currentGroups = graphEditor.GraphPresenter.GroupViewsCount;
            var currentNodes = model.Graph.NodesCount;
            var currentLinks = model.Graph.LinksCount;
            var currentGroups = model.EditorGraph.GroupsCount;
            
            nodesCount.text = $"Nodes count: {currentNodes}";
            linksCount.text = $"Links count: {currentLinks}";
            groupsCount.text = $"Groups count: {currentGroups}";
            unsavedNodesCount.text = $"Unsaved diff nodes count: {currentNodes - savedNodes}";
            unsavedLinksCount.text = $"Unsaved diff links count: {currentLinks - savedLinks}";
            unsavedGroupsCount.text = $"Unsaved diff groups count: {currentGroups - savedGroups}";
        }
    }
}