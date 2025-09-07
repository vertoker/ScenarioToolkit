using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Scenario.Core.Model.Interfaces;
using Scenario.Editor.Model;
using ScenarioToolkit.Core.Nodes;
using ScenarioToolkit.Core.Serialization;
using ScenarioToolkit.Editor.Utilities;
using ScenarioToolkit.Editor.Windows.GraphEditor.GraphViews;
using ScenarioToolkit.Shared;
using ScenarioToolkit.Shared.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using ZLinq;

namespace ScenarioToolkit.Editor.Windows.GraphEditor
{
    /// <summary>
    /// Контроллер модели, отвечает за хранение актуальной модели графа,
    /// а также за основные функции взаимодействия с ним
    /// </summary>
    public class ScenarioGraphController
    {
        private const float PasteOffset = 20;

        public event Action<IConditionNode> ConditionNodeSkipped;
        public event Action<ISubgraphNode> SubgraphNodeSkipped;
        
        public event Action<IScenarioNodeFlow> NodeActivated;
        /// <summary> Если сценарий надо сделать dirty </summary>
        public event Action GraphChanged;
        public event Action GraphLoaded;
        
        private const string PasteOperation = "Paste";
        private const string DuplicateOperation = "Duplicate";

        private readonly Dictionary<int, ScenarioNodeView> nodeViews;
        private readonly Dictionary<int, ScenarioLinkView> linkViews;
        private readonly BiListedDictionary<ScenarioGroupView, IScenarioGraphElement> groupElements;

        public IReadOnlyDictionary<int, ScenarioNodeView> NodeViews => nodeViews;
        public IReadOnlyDictionary<int, ScenarioLinkView> LinkViews => linkViews;
        
        public IReadOnlyDictionary<ScenarioGroupView, HashSet<IScenarioGraphElement>> GroupToElements => groupElements.GetKeyToListedValue();
        public IReadOnlyDictionary<IScenarioGraphElement, ScenarioGroupView> ElementToGroup => groupElements.ListedValueToKey;
        public IReadOnlyCollection<IScenarioGraphElement> GetGroupElements(ScenarioGroupView groupView) 
            => groupElements.GetReadOnlyListedValues(groupView);
        
        public IEnumerable<ScenarioNodeView> NodeViewsValues => NodeViews.Values;
        public IEnumerable<ScenarioLinkView> LinkViewsValues => LinkViews.Values;
        public IEnumerable<ScenarioGroupView> GroupViewsValues => groupElements.Keys;
        
        public int NodeViewsCount => NodeViews.Count;
        public int LinkViewsCount => LinkViews.Count;
        public int GroupViewsCount => groupElements.CountKeys;
        public int GroupElementsViewsCount => groupElements.CountKeys;
        
        public IScenarioGraph Graph { get; private set; } = IScenarioGraph.CreateNew();
        public ScenarioLoadService LoadService { get; }
        public ScenarioGraphView View { get; }

        public ScenarioGraphController(ScenarioGraphView view, ScenarioLoadService loadService)
        {
            View = view;
            LoadService = loadService;

            nodeViews = new Dictionary<int, ScenarioNodeView>();
            linkViews = new Dictionary<int, ScenarioLinkView>();
            groupElements = new BiListedDictionary<ScenarioGroupView, IScenarioGraphElement>();
            
            view.unserializeAndPaste += DeserializeAndPaste;
            view.serializeGraphElements += SerializeAndCopy;
            view.graphViewChanged += HandleChanges;
            view.elementsAddedToGroup += ElementsAddedToGroup;
            view.elementsRemovedFromGroup += ElementsRemovedFromGroup;
        }

        #region Serialize/Deserialize
        private string SerializeAndCopy(IEnumerable<GraphElement> elements) // copy
        {
            var copyModel = CopyUtils.CreateCopyModel(elements, this);
            var data = LoadService.SerializationService.Serialize(copyModel);
            return data;
        }
        private void DeserializeAndPaste(string operation, string data) // paste
        {
            if (operation is PasteOperation or DuplicateOperation)
                CopyAdd(data);
            else Debug.LogWarning($"Unknown operation {operation}");
        }
        
        public void Load(string data)
        {
            IScenarioModel scenarioModel;
            if (string.IsNullOrWhiteSpace(data))
                scenarioModel = IScenarioModel.CreateNew();
            else if (!LoadService.TryLoadModelFromJson(data, false, out scenarioModel))
                scenarioModel = IScenarioModel.CreateNew();
            
            Load(scenarioModel);
        }
        public void CopyAdd(string data)
        {
            CopyModel copyModel;
            if (string.IsNullOrWhiteSpace(data))
                copyModel = new CopyModel();
            else if (!LoadService.TryLoadFromJson(data, out copyModel))
                copyModel = new CopyModel();
            
            CopyAdd(copyModel);
        }

        public void Load(IScenarioModel model)
        {
            Graph = model.Graph;
            //Graph.Clear();
            
            var editorToViewsNodes = new Dictionary<IEditorNode, ScenarioNodeView>();
            
            // Editor graph
            foreach (var editorNode in model.EditorGraph.NodesValuesAVE)
            {
                var position = editorNode.Position;
                var content = editorNode.GetContent();
                var nodeView = CreateEditorNode(position, content, editorNode.Node, false, model.Context);
                editorToViewsNodes.Add(editorNode, nodeView);
            }
            foreach (var editorLink in model.EditorGraph.LinksValuesAVE)
            {
                var from = editorToViewsNodes[editorLink.From];
                var to = editorToViewsNodes[editorLink.To];
                CreateEditorLink(from, to);
            }
            foreach (var editorGroup in model.EditorGraph.GroupsValuesAVE)
            {
                var groupView = CreateEditorGroup(editorGroup);
                
                foreach (var editorNode in editorGroup.Nodes.Select(node => editorToViewsNodes[node]))
                    groupView.AddElement(editorNode);
            }
            
            GraphLoaded?.Invoke();
        }
        public void CopyAdd(CopyModel copyModel)
        {
            var portNodes = new List<IScenarioNode>(copyModel.EditorGraph.NodesCount);
            
            // Regenerate hashes for new nodes
            foreach (var node in copyModel.EditorGraph.NodesValuesAVE.Select(en => en.Node))
            {
                node.InitializeHash();
                
                if (node is IPortInNode or IPortOutNode)
                    portNodes.Add(node);
            }
            
            // Update PortNodes
            foreach (var portNode in portNodes)
            {
                if (portNode is IPortInNode inNode)
                {
                    if (!portNodes.Contains(inNode.OutputNode))
                        inNode.OutputNode = null;
                }
                else if (portNode is IPortOutNode outNode)
                {
                    if (!portNodes.Contains(outNode.InputNode))
                        outNode.InputNode = null;
                }
            }
            
            var nodeCenter = copyModel.EditorGraph.NodesValuesAVE.GetCenter(copyModel.EditorGraph.NodesCount);
            var screenCenter = View.GetScreenCenter();
            View.ClearSelection();
            
            var editorToViewsNodes = new Dictionary<IEditorNode, ScenarioNodeView>();
            
            // Editor Graph
            foreach (var editorNode in copyModel.EditorGraph.NodesValuesAVE)
            {
                var position = editorNode.Position;
                position += new Vector2(PasteOffset, PasteOffset);
                position = position - nodeCenter - screenCenter;
                
                var content = editorNode.GetContent();
                var nodeView = CreateEditorNode(position, content, editorNode.Node, true);
                editorToViewsNodes.Add(editorNode, nodeView);
                
                View.AddToSelection(nodeView);
            }
            foreach (var editorLink in copyModel.EditorGraph.LinksValuesAVE)
            {
                var from = editorToViewsNodes[editorLink.From];
                var to = editorToViewsNodes[editorLink.To];
                var linkView = CreateEditorLink(from, to);
                
                View.AddToSelection(linkView);
            }
            foreach (var editorGroup in copyModel.EditorGraph.GroupsValuesAVE)
            {
                var groupView = CreateEditorGroup(editorGroup, true);
                
                foreach (var editorNode in editorGroup.Nodes.Select(node => editorToViewsNodes[node]))
                    groupView.AddElement(editorNode);
            }
            
            // Invisible Graph
            foreach (var link in copyModel.InvisibleGraph.LinksValuesAVE)
            {
                //Debug.Log($"Invisible link with {link.From.Hash} and {link.To.Hash}");
                Graph.AddLink(link);
            }
            /*foreach (var editorNode in copyEditorGraph.Nodes)
            {
                if (editorNode.Node is IPortInNode inNode)
                {
                    var link = sourceGraph.Links.FirstOrDefault(link => link.From == inNode);
                    if (link == null) continue;
                    
                    var linkedOutNode = copyEditorGraph.Nodes.FirstOrDefault(en => en.Node == link.To);
                    if (linkedOutNode == null)
                        outNode.InputNode = null;
                }
                else if (editorNode.Node is IPortOutNode outNode)
                {
                    var link = sourceGraph.Links.FirstOrDefault(link => link.To == outNode);
                    if (link == null) continue;
                    
                    var linkedInNode = copyEditorGraph.Nodes.FirstOrDefault(en => en.Node == link.From);
                    if (linkedInNode == null)
                        outNode.InputNode = null;
                }
            }*/
        }
        #endregion

        #region Editor Graph Factory
        public ScenarioNodeView CreateEditorNode(Vector2 position, IEditorNodeContent content, 
            IScenarioNode sourceNode, bool setDirty = false, [CanBeNull] IScenarioContext context = null)
        {
            sourceNode ??= content.CreateDefault();
            
            var editorNode = new ScenarioNodeView(this, View);
            editorNode.Initialize(sourceNode, position, content, context);
            editorNode.AddManipulator(new ContextualMenuManipulator(populateMenu =>
            {
                if (sourceNode is IScenarioNodeFlow flowNode)
                {
                    if (flowNode is IConditionNode conditionNode)
                        populateMenu.menu.AppendAction("Skip", _ => { ConditionNodeSkipped?.Invoke(conditionNode); });
                    if (flowNode is ISubgraphNode subgraphNode)
                        populateMenu.menu.AppendAction("Complete", _ => { SubgraphNodeSkipped?.Invoke(subgraphNode); });
                    
                    populateMenu.menu.AppendAction("Activate", _ => { NodeActivated?.Invoke(flowNode); });
                    populateMenu.menu.AppendSeparator();
                }
            }));
            
            Graph.AddNode(sourceNode);
            nodeViews.Add(sourceNode.Hash, editorNode);
            View.AddElement(editorNode);
            
            EditorNodeCreated?.Invoke(editorNode);
            if (setDirty) GraphChanged?.Invoke();
            return editorNode;
        }
        public void RemoveEditorNode(ScenarioNodeView editorNode)
        {
            editorNode.Dispose();
            Graph.RemoveNode(editorNode.ScenarioNode);
            nodeViews.Remove(editorNode.ScenarioNode.Hash);
            View.RemoveElement(editorNode);
            
            EditorNodeRemoved?.Invoke(editorNode);
        }

        public ScenarioLinkView CreateEditorLink(ScenarioNodeView from, ScenarioNodeView to)
        {
            var fromNode = from.ScenarioNode as IScenarioNodeFlow;
            var toNode = to.ScenarioNode as IScenarioNodeFlow;
            
            var link = Graph.AddNewLink(fromNode, toNode);
            // Prevent to create existed link
            if (linkViews.TryGetValue(link.Hash, out var linkView)) return linkView;
            
            var viewLink = from.Output.ConnectTo<ScenarioLinkView>(to.Input);
            viewLink.Construct(link);
            linkViews.Add(link.Hash, viewLink);
            View.AddElement(viewLink);
            
            EditorLinkCreated?.Invoke(viewLink);
            return viewLink;
        }
        public void RemoveEditorLink(ScenarioLinkView linkView)
        {
            Graph.RemoveLink(linkView.ScenarioLink);
            linkViews.Remove(linkView.ScenarioLink.Hash);
            View.RemoveElement(linkView);
            
            EditorLinkRemoved?.Invoke(linkView);
        }

        public ScenarioGroupView CreateEditorGroup(IEditorGroup sourceGroup, bool setDirty = false)
        {
            sourceGroup ??= IEditorGroup.CreateNew();
            var editorGroup = new ScenarioGroupView(sourceGroup);
            
            groupElements.AddKey(editorGroup);
            View.AddElement(editorGroup);
            
            EditorGroupCreated?.Invoke(editorGroup);
            if (setDirty) GraphChanged?.Invoke();
            return editorGroup;
        }
        public void RemoveEditorGroup(ScenarioGroupView editorGroup)
        {
            groupElements.RemoveKey(editorGroup);
            View.RemoveElement(editorGroup);
            
            EditorGroupRemoved?.Invoke(editorGroup);
        }

        public event Action<ScenarioNodeView> EditorNodeCreated;
        public event Action<ScenarioNodeView> EditorNodeRemoved;
        public event Action<ScenarioLinkView> EditorLinkCreated;
        public event Action<ScenarioLinkView> EditorLinkRemoved;
        public event Action<ScenarioGroupView> EditorGroupCreated;
        public event Action<ScenarioGroupView> EditorGroupRemoved;

        public void InsertEditorNode(ScenarioLinkView editorLink, ScenarioNodeView newEditorNode)
        {
            var firstEditorNode = nodeViews[editorLink.ScenarioLink.From.Hash];
            var lastEditorNode = nodeViews[editorLink.ScenarioLink.To.Hash];
            
            RemoveEditorLink(editorLink);
            CreateEditorLink(firstEditorNode, newEditorNode);
            CreateEditorLink(newEditorNode, lastEditorNode);
        }
        public void InsertEditorInOutNodes(ScenarioLinkView editorLink, 
            ScenarioNodeView inEditorNode, ScenarioNodeView outEditorNode,
            out ScenarioNodeView firstEditorNode, out ScenarioNodeView lastEditorNode)
        {
            firstEditorNode = nodeViews[editorLink.ScenarioLink.From.Hash];
            lastEditorNode = nodeViews[editorLink.ScenarioLink.To.Hash];
            
            RemoveEditorLink(editorLink);
            CreateEditorLink(firstEditorNode, inEditorNode);
            CreateEditorLink(outEditorNode, lastEditorNode);
        }

        public void ReplaceEditorNode(ScenarioNodeView currentNode, ScenarioNodeView newNode)
        {
            if (currentNode.ScenarioNode is not IScenarioNodeFlow currentFlowNode) return;
            
            var links = Graph.GetAllLinksAVE(currentFlowNode).Select(f => linkViews[f.Hash]).ToArray();
            var incomingNodes = Graph.GetIncomingNodesAVE(currentFlowNode).Select(n => nodeViews[n.Hash]).ToArray();
            var outcomingNodes = Graph.GetOutcomingNodesAVE(currentFlowNode).Select(n => nodeViews[n.Hash]).ToArray();
            
            RemoveEditorNode(currentNode);
            foreach (var link in links)
                RemoveEditorLink(link);
            
            foreach (var incomeNode in incomingNodes)
                CreateEditorLink(incomeNode, newNode);
            foreach (var outcomeNode in outcomingNodes)
                CreateEditorLink(newNode, outcomeNode);
        }
        public void UpdateNodeHash(ScenarioNodeView nodeView, int oldHash, int newHash)
        {
            var links = nodeView.ScenarioNode is IScenarioNodeFlow flowNode 
                ? Graph.GetAllLinksAVE(flowNode).ToArray() : Array.Empty<IScenarioLinkFlow>();
            var oldHashes = links.Select(l => l.Hash).ToArray();
            
            Graph.UpdateHash(nodeView.ScenarioNode, oldHash, newHash);
            
            nodeViews.Remove(oldHash);
            nodeViews.Add(newHash, nodeView);

            foreach (var linkHash in oldHashes)
            {
                linkViews.Remove(linkHash, out var linkView);
                linkViews.Add(linkView.ScenarioLink.Hash, linkView);
            }
        }
        
        public void Clear()
        {
            //Graph.Clear();
            linkViews.Clear();
            nodeViews.Clear();
            groupElements.Clear();
            
            foreach (var element in View.graphElements.ToList())
                View.RemoveElement(element);
        }
        #endregion

        #region Other
        // TODO Undo/Redo буффер
        private GraphViewChange HandleChanges(GraphViewChange change)
        {
            // Create
            change.edgesToCreate?.ForEach(edge =>
            {
                var fromElement = (ScenarioNodeView)edge.output.node;
                var toElement = (ScenarioNodeView)edge.input.node;
                CreateEditorLink(fromElement, toElement);
            });
            change.edgesToCreate?.Clear();
            
            // Edit
            //var nodesToUpdate = change.movedElements?.OfType<ScenarioNodeView>().ToList();

            // Remove
            var nodesToRemove = change.elementsToRemove?.OfType<ScenarioNodeView>().ToList();
            var adjacentEdges = nodesToRemove?.SelectMany(GetAdjacentEdges);
            var edgesToRemove = change.elementsToRemove?.OfType<ScenarioLinkView>().Concat(adjacentEdges).Distinct().ToList();
            var groupsToRemove = change.elementsToRemove?.OfType<ScenarioGroupView>().ToList();

            nodesToRemove?.ForEach(RemoveEditorNode);
            edgesToRemove?.ForEach(RemoveEditorLink);
            groupsToRemove?.ForEach(RemoveEditorGroup);

            change.elementsToRemove = change.elementsToRemove?.Except(nodesToRemove).Except(edgesToRemove).ToList();
            
            GraphChanged?.Invoke();
            return change;
        }
        private void ElementsAddedToGroup(Group group, IEnumerable<GraphElement> elements)
        {
            var editorGroup = (ScenarioGroupView)group;
            foreach (var element in elements.OfType<IScenarioGraphElement>())
                groupElements.AddListedValue(editorGroup, element);
        }
        private void ElementsRemovedFromGroup(Group group, IEnumerable<GraphElement> elements)
        {
            foreach (var element in elements.OfType<IScenarioGraphElement>())
                groupElements.RemoveListedValue(element);
        }

        private static IEnumerable<ScenarioLinkView> GetAdjacentEdges(ScenarioNodeView node)
        {
            var adjacentEdges = new List<ScenarioLinkView>();
            adjacentEdges.AddRange(node.Input?.connections.Cast<ScenarioLinkView>() ?? Array.Empty<ScenarioLinkView>());
            adjacentEdges.AddRange(node.Output?.connections.Cast<ScenarioLinkView>() ?? Array.Empty<ScenarioLinkView>());
            return adjacentEdges;
        }
        public void UpdateVisuals()
        {
            foreach (var scenarioNodeView in NodeViewsValues)
                scenarioNodeView.UpdateVisuals();
        }
        #endregion
    }
}