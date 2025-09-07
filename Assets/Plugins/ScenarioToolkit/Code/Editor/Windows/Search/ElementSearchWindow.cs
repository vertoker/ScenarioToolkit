using System.Collections.Generic;
using ModestTree;
using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Core.Nodes;
using ScenarioToolkit.Editor.Utilities;
using ScenarioToolkit.Editor.Windows.GraphEditor;
using ScenarioToolkit.Editor.Windows.GraphEditor.GraphViews;
using ScenarioToolkit.Editor.Windows.Search.General;
using ScenarioToolkit.Shared;
using UnityEditor;
using UnityEditor.Searcher;
using ElementSearchItem = ScenarioToolkit.Editor.Windows.Search.General.SearchItem<ScenarioToolkit.Editor.Windows.GraphEditor.GraphViews.IScenarioGraphElement>;

namespace ScenarioToolkit.Editor.Windows.Search
{
    /// <summary>
    /// Поисковик элементов в сценарии, использует экспериментальный пакет Searcher
    /// </summary>
    public class ElementSearchWindow : BaseSearchWindow
    {
        private const int GraphElementItems = 16;
        private const int ComponentItems = 8;

        private const string NodeIndex = "NodeIndex:";
        private const string NodeName = "NodeName:";
        private const string NodeHash = "NodeHash:";
        private const string NodeType = "NodeType:";
        
        private const string ComponentIndex = "ComponentIndex:";
        private const string ComponentType = "ComponentType:";
        private const string FieldValue = "FieldValue:";
        
        private const string LinkIndex = "LinkIndex:";
        private const string LinkHash = "LinkHash:";
        private const string NodeFrom = "NodeFrom:";
        private const string NodeTo = "NodeTo:";
        
        private const string GroupIndex = "GroupIndex:";
        private const string GroupName = "GroupName:";
        private const string GroupHash = "GroupHash:";
        private const string GroupCount = "GroupCount:";
        private const string GroupNodeHash = "GroupNodeHash:";
        
        private readonly List<ElementSearchItem> childrenCache = new(GraphElementItems);
        private readonly List<ElementSearchItem> componentCache = new(ComponentItems);

        public ElementSearchWindow(GraphEditorWindow graphEditor) : base(graphEditor, "Element Searcher", 2048) { }

        public override void Open()
        {
            UpdateRoot();
            Database.ClearAddItems(Root);
            
            var displayPosition = (GraphEditor as EditorWindow).GetLocalAnchorPoint(EditorWindowUtils.Anchor.Center);
            SearcherWindow.Show(GraphEditor as EditorWindow, Searcher, ItemSelectedDelegate, displayPosition, null, Alignment);
        }
        public override bool ItemSelectedDelegate(SearcherItem item)
        {
            if (item is ElementSearchItem elemItem)
            {
                GraphEditor.GraphView.Show(elemItem.ContextItem);
                GraphEditor.GraphView.SelectOnly(elemItem.ContextItem);
                return true;
            }
            return false;
        }

        private void UpdateRoot()
        {
            Root.Clear();
            
            AddNodes();
            AddLinks();
            AddGroup();
        }
        
        #region Add
        private void AddNodes()
        {
            var indexCounter = 0;
            foreach (var nodeView in GraphEditor.GraphController.NodeViewsValues)
            {
                childrenCache.Clear();
                BuildNode(nodeView);
                
                //var parent = pool.Get().Initialize(nodeView, $"{NodeIndex}{indexCounter++}");
                //InternalBridge.OverwriteDatabase(parent, null);
                var parent = new ElementSearchItem(nodeView, $"{NodeIndex}{indexCounter++}");
                parent.AddChildren(childrenCache);
                Root.Add(parent);
            }
        }
        private void AddLinks()
        {
            var indexCounter = 0;
            foreach (var linkView in GraphEditor.GraphController.LinkViewsValues)
            {
                childrenCache.Clear();
                BuildLink(linkView);
                var parent = new ElementSearchItem(linkView, $"{LinkIndex}{indexCounter++}");
                parent.AddChildren(childrenCache);
                Root.Add(parent);
            }
        }
        private void AddGroup()
        {
            var indexCounter = 0;
            foreach (var groupBind in GraphEditor.GraphController.GroupToElements)
            {
                childrenCache.Clear();
                BuildGroup(groupBind.Key, groupBind.Value);
                var parent = new ElementSearchItem(groupBind.Key, $"{GroupIndex}{indexCounter++}");
                parent.AddChildren(childrenCache);
                Root.Add(parent);
            }
        }
        #endregion

        #region Build
        private void BuildNode(ScenarioNodeView nodeView)
        {
            var node = nodeView.ScenarioNode;

            if (!string.IsNullOrWhiteSpace(node.Name))
                childrenCache.Add(new ElementSearchItem(nodeView, $"{NodeName}{node.Name}"));
            childrenCache.Add(new ElementSearchItem(nodeView, $"{NodeHash}{node.Hash}"));
            var content = node.GetContent();
            childrenCache.Add(new ElementSearchItem(nodeView, $"{NodeType}{content.TypeName}"));

            if (node is IScenarioNodeComponents componentsNode)
            {
                //childrenCache.Add(new ElementSearchItem(nodeView, $"{ComponentsCount}{componentsNode.ComponentsCount}"));
                
                using var enumerator = componentsNode.GetEnumerator();
                var counter = 0;
                
                while (enumerator.MoveNext())
                {
                    var component = enumerator.Current;
                    if (component == null) continue;
                    
                    componentCache.Clear();
                    BuildComponent(nodeView, component);
                    var parent = new ElementSearchItem(nodeView, $"{ComponentIndex}{counter++}");
                    parent.AddChildren(componentCache);
                    childrenCache.Add(parent);
                }
            }
        }
        private void BuildComponent(ScenarioNodeView nodeView, IScenarioComponent component)
        {
            componentCache.Add(new ElementSearchItem(nodeView, $"{ComponentType}{component.GetType().PrettyName()}"));
            
            foreach (var propValue in component.GetValues())
                componentCache.Add(new ElementSearchItem(nodeView, $"{FieldValue}{propValue.GetScenarioFieldContent()}"));
        }
        private void BuildLink(ScenarioLinkView linkView)
        {
            var fromHash = linkView.ScenarioLink.From.Hash;
            var toHash = linkView.ScenarioLink.To.Hash;
            var fromNodeView = GraphEditor.GraphController.NodeViews[fromHash];
            var toNodeView = GraphEditor.GraphController.NodeViews[toHash];
            
            childrenCache.Add(new ElementSearchItem(linkView, $"{LinkHash}{linkView.ScenarioLink.Hash}"));
            childrenCache.Add(new ElementSearchItem(fromNodeView, $"{NodeFrom}{fromHash}"));
            childrenCache.Add(new ElementSearchItem(toNodeView, $"{NodeTo}{toHash}"));
        }
        private void BuildGroup(ScenarioGroupView groupView, HashSet<IScenarioGraphElement> groupElements)
        {
            if (!string.IsNullOrWhiteSpace(groupView.Name))
                childrenCache.Add(new ElementSearchItem(groupView, $"{GroupName}{groupView.Name}"));
            
            childrenCache.Add(new ElementSearchItem(groupView, $"{GroupHash}{groupView.Hash}, {GroupCount}{groupElements.Count}"));
            foreach (var groupElement in groupElements)
            {
                var nodeView = (ScenarioNodeView)groupElement;
                childrenCache.Add(new ElementSearchItem(nodeView, $"{GroupNodeHash}{nodeView.ScenarioNode.Hash}"));
            }
        }
        #endregion
        
    }
}