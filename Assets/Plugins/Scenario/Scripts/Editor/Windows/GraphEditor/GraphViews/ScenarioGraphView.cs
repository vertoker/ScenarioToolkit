using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Scenario.Core.Model;
using Scenario.Core.Model.Interfaces;
using Scenario.Core.Nodes;
using Scenario.Core.Player;
using Scenario.Editor.Model;
using Scenario.Editor.Utilities;
using Scenario.Editor.Utilities.Providers;
using Scenario.Utilities;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using ZLinq;

namespace Scenario.Editor.Windows.GraphEditor.GraphViews
{
    public enum ScenarioGraphElementState
    {
        Default,
        Active,
        Completed
    }

    public class ScenarioGraphView : GraphView
    {
        private readonly GraphEditorWindow graphEditor;
        private ScenarioGraphController controller;
        private ScenarioPlayer player;

        private const float MinZoom = 0.02f;
        private const float MaxZoom = 2.5f;
        private const float ZoomStep = 0.15f;
        private const float ReferenceZoom = 1f;
        
        public ScenarioGraphView(GraphEditorWindow graphEditor)
        {
            this.graphEditor = graphEditor;
            
            AddStyles();
            AddGridBackground();
            SetupZoom(MinZoom, MaxZoom, ZoomStep, ReferenceZoom);
            AddManipulators();
            this.StretchToParentSize();
        }

        public void SetPresenter(ScenarioGraphController scenarioGraphController) => controller = scenarioGraphController;
        public void SetEditorSession(ScenarioGraphEditorSession newEditorSession)
        {
        }

        public bool IsEmpty() => !graphElements.Any();
        
        // Highlight Nodes Zone
        
        public void SetPlayer(ScenarioPlayer newPlayer)
        {
            player = newPlayer;
            // Apply Nodes
            foreach (var activeNode in player.ActiveNodesAVE)
                SetNodeActive(activeNode);
            foreach (var completedNode in player.CompletedNodesAVE)
                SetNodeCompleted(completedNode);

            if (newPlayer != null)
            {
                newPlayer.NodeAfterActivated += SetNodeActive;
                newPlayer.NodeAfterCompleted += SetNodeCompleted;
            }
        }
        public void ResetPlayer()
        {
            if (player != null)
            {
                player.NodeAfterActivated -= SetNodeActive;
                player.NodeAfterCompleted -= SetNodeCompleted;
            }
            
            // Reset Nodes
            foreach (var element in nodes.OfType<ScenarioNodeView>())
                SetElement(element, ScenarioGraphElementState.Default);
            player = null;
        }
        
        private void SetNodeActive(IScenarioNode node) => SetNodeState(node, ScenarioGraphElementState.Active);
        private void SetNodeCompleted(IScenarioNode node) => SetNodeState(node, ScenarioGraphElementState.Completed);

        private void SetNodeState(IScenarioNode node, ScenarioGraphElementState state)
        {
            var elements = nodes.OfType<ScenarioNodeView>().Where(element => element.ScenarioNode.Hash == node.Hash);
            
            foreach (var element in elements)
                SetElement(element, state);
        }

        private static void SetElement(ScenarioNodeView element, ScenarioGraphElementState state)
        {
            switch (state)
            {
                case ScenarioGraphElementState.Default:
                    element.RemoveFromClassList("active-node");
                    element.RemoveFromClassList("completed-node");
                    break;
                case ScenarioGraphElementState.Active:
                    element.AddToClassList("active-node");
                    element.RemoveFromClassList("completed-node");
                    break;
                case ScenarioGraphElementState.Completed:
                    element.RemoveFromClassList("active-node");
                    element.AddToClassList("completed-node");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
            element.UpdateVisuals();
        }
        
        //
        
        public void Show(IScenarioGraphElement graphElement)
        {
            Show(graphElement.GetCenterPosition());
        }
        public void Show(Vector2 position)
        {
            var center = -position * viewTransform.scale + GetScreenCenterOffset();
            viewTransform.position = new Vector3(center.x, center.y, 0);
        }
        
        public GraphCameraPose GetCamera()
        {
            var pose = new GraphCameraPose(viewTransform.position, viewTransform.scale);
            return pose;
        }
        public void SetCamera(GraphCameraPose pose)
        {
            viewTransform.position = pose.Position;
            viewTransform.scale = pose.Scale;
        }

        public Vector2 GetScreenCenter()
        {
            if (viewTransform.scale.x == 0 || viewTransform.scale.y == 0 || viewTransform.scale.z == 0)
                return Vector2.zero;
            return ((Vector2)viewTransform.position - GetScreenCenterOffset()) / viewTransform.scale;
        }
        private Vector2 GetScreenCenterOffset()
        {
            var size = localBound.size;
            if (float.IsNaN(size.x) || float.IsNaN(size.y))
                size = Vector2.one;
            return size / 2;
        }
        
        public Vector2 GetGraphCenter()
        {
            var scenarioNodes = nodes.OfType<IScenarioGraphElement>().ToArray();
            if (scenarioNodes.Length == 0) return Vector2.zero;

            var positionSum = scenarioNodes.Aggregate(Vector2.zero, 
                (current, scenarioNode) => current + scenarioNode.GetCenterPosition());

            return positionSum / scenarioNodes.Length;
        }

        public void MoveInstantly(Vector2 delta)
        {
            var pos = viewTransform.position;
            pos.x += delta.x;
            pos.y += delta.y;
            viewTransform.position = pos;
        }

        public void SelectOnly(params ISelectable[] selectables)
        {
            ClearSelection();
            foreach (var selectable in selectables)
                AddToSelection(selectable);
        }

        public void SetConditionCompleted(ScenarioNode node, IScenarioCondition condition)
        {
            var elements = nodes.OfType<ScenarioNodeView>().Where(element => element.ScenarioNode == node);
            foreach (var element in elements)
                element.SetConditionCompleted(condition);
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.Where(port => port != startPort
                                       && port.direction != startPort.direction
                                       && port.node != startPort.node).ToList();
        }

        private void AddStyles()
        {
            styleSheets.Add(UIProvider.GetUssSheet("GraphEditor"));
        }

        private void AddManipulators()
        {
            // Default
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            // Custom
            this.AddManipulator(CreateContextualMenuManipulator());
        }

        private IManipulator CreateContextualMenuManipulator()
        {
            return new ContextualMenuManipulator(
                populateMenu =>
                {
                    populateMenu.menu.AppendSeparator();

                    if (selection.Count == 0)
                    {
                        AddNode<IActionNode>(populateMenu, "Add Core Node/Action Node");
                        AddNode<IConditionNode>(populateMenu, "Add Core Node/Condition Node");
                        AddNode<ISubgraphNode>(populateMenu, "Add Core Node/Subgraph Node");
                        AddNode<IStartNode>(populateMenu, "Add Core Node/Start Node");
                        AddNode<IEndNode>(populateMenu, "Add Core Node/End Node");
                    
                        AddPortNodes(populateMenu, "Add Utility Node/Port Nodes");
                        AddNode<INoteNode>(populateMenu, "Add Utility Node/Sticky Node");
                    
                        LoadPreset(populateMenu, "Load Preset/Basic", "Basic");
                        LoadPreset(populateMenu, "Load Preset/Actn-Cond", "Action-Condition");
                        LoadPreset(populateMenu, "Load Preset/Cond-Actn", "Condition-Action");
                    }
                    else
                    {
                        switch (selection[0])
                        {
                            case ScenarioLinkView editorLink:
                                InsertNodeInLink<IActionNode>(populateMenu, editorLink,
                                    "Insert Core Node (Link)/Action Node");
                                InsertNodeInLink<IConditionNode>(populateMenu, editorLink,
                                    "Insert Core Node (Link)/Condition Node");
                                InsertNodeInLink<ISubgraphNode>(populateMenu, editorLink,
                                    "Insert Core Node (Link)/Subgraph Node");
                                InsertNodeInLink<IStartNode>(populateMenu, editorLink,
                                    "Insert Core Node (Link)/Start Node");
                                InsertNodeInLink<IEndNode>(populateMenu, editorLink,
                                    "Insert Core Node (Link)/End Node");

                                InsertPortNodesInLink(populateMenu, editorLink,
                                    "Insert Utility Node (Link)/Port Nodes");
                                break;
                            
                            case ScenarioNodeView editorNode:
                                AddContinuousNode<IActionNode>(populateMenu, editorNode,
                                    "Add Continuous Node (Node)/Action Node");
                                AddContinuousNode<IConditionNode>(populateMenu, editorNode,
                                    "Add Continuous Node (Node)/Condition Node");
                                AddContinuousNode<ISubgraphNode>(populateMenu, editorNode,
                                    "Add Continuous Node (Node)/Subgraph Node");
                                AddContinuousNode<IEndNode>(populateMenu, editorNode,
                                    "Add Continuous Node (Node)/End Node");

                                ReplaceNode<IActionNode>(populateMenu, editorNode, "Replace Node (Node)/Action Node");
                                ReplaceNode<IConditionNode>(populateMenu, editorNode,
                                    "Replace Node (Node)/Condition Node");
                                ReplaceNode<ISubgraphNode>(populateMenu, editorNode,
                                    "Replace Node (Node)/Subgraph Node");
                                ReplaceNode<IStartNode>(populateMenu, editorNode, "Replace Node (Node)/Start Node");
                                ReplaceNode<IEndNode>(populateMenu, editorNode, "Replace Node (Node)/End Node");
                                break;
                            
                            case ScenarioGroupView editorGroup:
                                InsertNodeInGroup<IActionNode>(populateMenu, editorGroup,
                                    "Insert Core Node (Group)/Action Node");
                                InsertNodeInGroup<IConditionNode>(populateMenu, editorGroup,
                                    "Insert Core Node (Group)/Condition Node");
                                InsertNodeInGroup<ISubgraphNode>(populateMenu, editorGroup,
                                    "Insert Core Node (Group)/Subgraph Node");
                                InsertNodeInGroup<IStartNode>(populateMenu, editorGroup,
                                    "Insert Core Node (Group)/Start Node");
                                InsertNodeInGroup<IEndNode>(populateMenu, editorGroup,
                                    "Insert Core Node (Group)/End Node");

                                InsertPortNodesInGroup(populateMenu, editorGroup,
                                    "Insert Utility Node (Group)/Port Nodes");
                                InsertNodeInGroup<INoteNode>(populateMenu, editorGroup,
                                    "Insert Utility Node (Group)/Sticky Node");

                                populateMenu.menu.AppendAction("Delete Only Group", actionEvent =>
                                {
                                    foreach (var scenarioElement in controller.GetGroupElements(editorGroup).ToArray())
                                        editorGroup.RemoveElement(scenarioElement.GetGraphElement());
                                    controller.RemoveEditorGroup(editorGroup);
                                });
                                break;
                        }
                    }

                    populateMenu.menu.AppendSeparator();
                    AddGroupActions(populateMenu);
                    AddSubgraphFromCopy(populateMenu, "Add Subgraph from Copy");
                }
            );
        }

        private static readonly Vector2 PortNodesOffset = new(100, 0);
        private static readonly Vector2 ContinuousNodeOffset = new(200, 0);
        private static readonly Vector2 InNodeOffset = new(150, 0);
        private static readonly Vector2 OutNodeOffset = new(100, 0);
        
        #region Node

        public void CreateEditorNode(Vector2 mousePosition, IEditorNodeContent content, out ScenarioNodeView editorNode) 
            => CreateEditorNodeGlobal(contentViewContainer.WorldToLocal(mousePosition), content, out editorNode);
        public void CreateEditorNodeGlobal(Vector2 position, IEditorNodeContent content, out ScenarioNodeView editorNode)
        {
            ClearSelection();
            editorNode = controller.CreateEditorNode(position, content, null, true);
            AddToSelection(editorNode);
        }
        private void AddNode<TNode>(ContextualMenuPopulateEvent populateMenu, 
            string text) where TNode : IScenarioNode
        {
            var content = NodesContent.GetContent<TNode>();
            populateMenu.menu.AppendAction(text, actionEvent => 
                { CreateEditorNode(actionEvent.eventInfo.localMousePosition, content, out _); });
        }
        private void InsertNodeInLink<TNode>(ContextualMenuPopulateEvent populateMenu, 
            ScenarioLinkView linkView, string text) where TNode : IScenarioNode
        {
            var content = NodesContent.ReadOnlyINodeBinds[typeof(TNode)];
            populateMenu.menu.AppendAction(text, actionEvent =>
            {
                CreateEditorNode(actionEvent.eventInfo.localMousePosition, content, out var editorNode);
                controller.InsertEditorNode(linkView, editorNode);
            });
        }
        private void InsertNodeInGroup<TNode>(ContextualMenuPopulateEvent populateMenu, 
            ScenarioGroupView groupView, string text) where TNode : IScenarioNode
        {
            var content = NodesContent.ReadOnlyINodeBinds[typeof(TNode)];
            populateMenu.menu.AppendAction(text, actionEvent =>
            {
                CreateEditorNode(actionEvent.eventInfo.localMousePosition, content, out var editorNode);
                groupView.AddElement(editorNode);
            });
        }
        private void ReplaceNode<TNode>(ContextualMenuPopulateEvent populateMenu, 
            ScenarioNodeView nodeView, string text) where TNode : IScenarioNode
        {
            var content = NodesContent.ReadOnlyINodeBinds[typeof(TNode)];
            populateMenu.menu.AppendAction(text, actionEvent =>
            {
                CreateEditorNodeGlobal(nodeView.GetOnlyPosition(), content, out var editorNode);
                controller.ReplaceEditorNode(nodeView, editorNode);
            });
        }
        private void AddContinuousNode<TNode>(ContextualMenuPopulateEvent populateMenu, 
            ScenarioNodeView startNodeView, string text) where TNode : IScenarioNode
        {
            var startContent = startNodeView.ScenarioNode.GetContent();
            var endContent = NodesContent.ReadOnlyINodeBinds[typeof(TNode)];

            // Если порты не подразумеваются для типов
            if (!startContent.HasOutput || !endContent.HasInput) return;
            
            populateMenu.menu.AppendAction(text, actionEvent =>
            {
                var position = startNodeView.GetOnlyPosition() + ContinuousNodeOffset;
                CreateEditorNodeGlobal(position, endContent, out var endNodeView);
                controller.CreateEditorLink(startNodeView, endNodeView);
            });
        }
        #endregion
        
        #region Port Nodes
        public void CreatePortNodes(Vector2 mousePosition, 
            IEditorNodeContent contentIn, IEditorNodeContent contentOut, 
            out ScenarioNodeView editorInNode, out ScenarioNodeView editorOutNode)
        {
            ClearSelection();

            var inNode = (IPortInNode)contentIn.CreateDefault();
            var outNode = (IPortOutNode)contentOut.CreateDefault();
            controller.Graph.AddNewLink(inNode, outNode);
            inNode.OutputNode = outNode;
            outNode.InputNode = inNode;
            
            var positionIn = contentViewContainer.WorldToLocal(mousePosition);
            var positionOut = positionIn + PortNodesOffset;
            editorInNode = controller.CreateEditorNode(positionIn, contentIn, inNode, true);
            editorOutNode = controller.CreateEditorNode(positionOut, contentOut, outNode, true);
                
            AddToSelection(editorInNode);
            AddToSelection(editorOutNode);
        }
        private void AddPortNodes(ContextualMenuPopulateEvent populateMenu, string text)
        {
            var contentIn = NodesContent.GetContent<IPortInNode>();
            var contentOut = NodesContent.GetContent<IPortOutNode>();
            
            populateMenu.menu.AppendAction(text, actionEvent => 
                { CreatePortNodes(actionEvent.eventInfo.localMousePosition, contentIn, contentOut, out _, out _); });
        }
        private void InsertPortNodesInLink(ContextualMenuPopulateEvent populateMenu, ScenarioLinkView linkView, string text)
        {
            var contentIn = NodesContent.GetContent<IPortInNode>();
            var contentOut = NodesContent.GetContent<IPortOutNode>();
            
            populateMenu.menu.AppendAction(text, actionEvent =>
            {
                CreatePortNodes(actionEvent.eventInfo.localMousePosition, 
                    contentIn, contentOut, out var editorInNode, out var editorOutNode);
                controller.InsertEditorInOutNodes(linkView, editorInNode, 
                    editorOutNode, out var firstEditorNode, out var lastEditorNode);
                
                editorInNode.SetOnlyPosition(firstEditorNode.GetOnlyPosition() + InNodeOffset);
                editorOutNode.SetOnlyPosition(lastEditorNode.GetOnlyPosition() - OutNodeOffset);
            });
        }
        private void InsertPortNodesInGroup(ContextualMenuPopulateEvent populateMenu, ScenarioGroupView groupView, string text)
        {
            var contentIn = NodesContent.GetContent<IPortInNode>();
            var contentOut = NodesContent.GetContent<IPortOutNode>();
            
            populateMenu.menu.AppendAction(text, actionEvent =>
            {
                CreatePortNodes(actionEvent.eventInfo.localMousePosition, 
                    contentIn, contentOut, out var editorInNode, out var editorOutNode);
                groupView.AddElement(editorInNode);
                groupView.AddElement(editorOutNode);
            });
        }
        #endregion

        #region Presets
        private void LoadPreset(ContextualMenuPopulateEvent populateMenu, string text, string presetName)
        {
            populateMenu.menu.AppendAction(text, actionEvent =>
            {
                ClearSelection();
                var json = PresetProvider.GetPresetText(presetName);
                var model = controller.LoadService.LoadModelFromJson(json);
                var copyModel = CopyUtils.CreateCopyModel(model, controller);
                
                var position = contentViewContainer.WorldToLocal(actionEvent.eventInfo.localMousePosition);
                foreach (var node in model.EditorGraph.NodesValuesAVE)
                    node.Position += position;

                controller.CopyAdd(copyModel);
            });
        }
        #endregion

        #region Group
        public const string DefaultGroupName = "New group";
        private void AddGroupActions(ContextualMenuPopulateEvent populateMenu)
        {
            populateMenu.menu.AppendAction("Add to Group", actionEvent =>
            {
                var position = contentViewContainer.WorldToLocal(actionEvent.eventInfo.localMousePosition);
                var sourceGroup = IEditorGroup.CreateNew();
                sourceGroup.Name = DefaultGroupName;
                sourceGroup.Position = position;
                
                var editorGroup = controller.CreateEditorGroup(sourceGroup, true);
                
                // TODO можно подумать над вложенными группами
                foreach (var editorNode in selection.OfType<ScenarioNodeView>())
                    editorGroup.AddElement(editorNode);
                AddToSelection(editorGroup);
            });

            populateMenu.menu.AppendAction("Remove from Group", actionEvent =>
            {
                var nodesToUngroup = selection.OfType<ScenarioNodeView>().ToList();
                var groupsToRemove = new HashSet<ScenarioGroupView>();
                foreach (var editorNode in nodesToUngroup)
                {
                    if (controller.ElementToGroup.TryGetValue(editorNode, out var editorGroup))
                    {
                        editorGroup.RemoveElement(editorNode);
                        groupsToRemove.Add(editorGroup);
                    }
                }
                foreach (var group in groupsToRemove)
                {
                    if (controller.GetGroupElements(group).Count == 0)
                        controller.RemoveEditorGroup(group);
                }
            });
        }
        #endregion

        #region Utility
        private void AddSubgraphFromCopy(ContextualMenuPopulateEvent populateMenu, string text)
        {
            var content = NodesContent.GetContent<ISubgraphNode>();
            populateMenu.menu.AppendAction(text, Action);
            return;

            async void Action(DropdownMenuAction actionEvent)
            {
                // TODO просто отвратительный код, надо переписать, чтобы он не открывал сценарии постоянно
                
                var savePath = EditorUtility.SaveFilePanel("Save New Subgraph Copy", 
                    DirectoryFileHelper.SaveDirectory, DirectoryFileHelper.GetNewFileName("new"), "json");
                if (string.IsNullOrWhiteSpace(savePath))
                {
                    Debug.Log("<b>Scenario cancelled</b>: empty selected path");
                    return;
                }
                
                var selected = selection.OfType<GraphElement>().ToArray();
                var copyModel = CopyUtils.CreateCopyModel(selected, controller);
                var sourceAsset = graphEditor.Session.TextAsset;

                //var position = selected.OfType<IScenarioGraphElement>().ToArray().GetCenter();
                var position = actionEvent.eventInfo.localMousePosition;
                foreach (var graphElement in selected) RemoveElement(graphElement);
                var editorNode = controller.CreateEditorNode(position, content, null, true);
                var subgraphHash = editorNode.ScenarioNode.Hash;
                graphEditor.Save();
                await Task.Yield();
                
                graphEditor.FileMenu.New();
                controller.CopyAdd(copyModel);
                graphEditor.SaveSession(savePath);
                var nextAsset = graphEditor.Session.TextAsset;
                await Task.Yield();
                
                graphEditor.FileMenu.Open(sourceAsset);
                Show(position);
                
                await Task.Yield();
                AssetDatabase.Refresh();
                
                var subgraphNode = (ISubgraphNode)controller.Graph.GetNode(subgraphHash);
                subgraphNode.Json = nextAsset;
                editorNode = controller.NodeViews[subgraphNode.Hash];
                editorNode.SetCenterPosition(position);
                editorNode.UpdateVisuals();
                
                graphEditor.Save();
            }
        }
        #endregion
        
        private void AddGridBackground()
        {
            var gridBackground = new GridBackground();
            gridBackground.StretchToParentSize();
            Insert(0, gridBackground);
        }
    }
}