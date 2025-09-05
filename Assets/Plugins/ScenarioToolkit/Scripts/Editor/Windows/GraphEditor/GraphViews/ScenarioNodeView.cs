using System.Collections.Generic;
using JetBrains.Annotations;
using ModestTree;
using Scenario.Core.Model;
using Scenario.Core.Model.Interfaces;
using Scenario.Core.Nodes;
using Scenario.Utilities;
using Scenario.Utilities.Attributes;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Scenario.Editor.Windows.GraphEditor.GraphViews
{
    public class ScenarioNodeView : Node, IScenarioGraphElement
    {
        private const string NullUSS = "null";
        
        private const string BaseNodeUSS = "scenario-node";
        private const string CompletedConditionUSS = "completed-condition";
        private const string SelectedNodeUSS = "selected-node";
        private const string LinkedNodeUSS = "linked-node";

        private readonly ScenarioGraphController graphController;
        private readonly ScenarioGraphView graphView;

        public IScenarioNode ScenarioNode { get; private set; }
        public IEditorNodeContent Content { get; private set; }
        [CanBeNull] private IScenarioContext Context { get; set; }

        public ScenarioPortView Input => Content.HasInput ? (ScenarioPortView)inputContainer[0] : null;
        public ScenarioPortView Output => Content.HasOutput ? (ScenarioPortView)outputContainer[0] : null;

        public Vector2 GetCenterPosition()
        {
            var rect = GetPosition();
            return rect.center;
        }
        public Vector2 GetOnlyPosition() => GetPosition().position;
        public GraphElement GetGraphElement() => this;

        public ScenarioNodeView(ScenarioGraphController graphController, ScenarioGraphView graphView)
        {
            this.graphController = graphController;
            this.graphView = graphView;
        }

        public void Initialize(IScenarioNode node, Vector2 position,
            IEditorNodeContent content, [CanBeNull] IScenarioContext context)
        {
            Content = content;
            Context = context;

            if (Content.HasInput) AddInputPort();
            if (Content.HasOutput) AddOutputPort();
            
            AddToClassList(BaseNodeUSS);
            Bind(node);
            
            SetPosition(new Rect(position, GetPosition().size));
        }
        public void Dispose()
        {
            switch (ScenarioNode)
            {
                case IPortInNode portInNode:
                    if (portInNode.OutputNode == null) return;
                    var editorOutNode = graphController.NodeViews[portInNode.OutputNode.Hash];
                    
                    graphController.Graph.RemoveLink(portInNode, portInNode.OutputNode);
                    portInNode.OutputNode.InputNode = null;
                    editorOutNode.UpdateVisuals();
                    break;
                case IPortOutNode portOutNode:
                    if (portOutNode.InputNode == null) return;
                    var editorInNode = graphController.NodeViews[portOutNode.InputNode.Hash];
                    
                    graphController.Graph.RemoveLink(portOutNode.InputNode, portOutNode);
                    portOutNode.InputNode.OutputNode = null;
                    editorInNode.UpdateVisuals();
                    break;
            }
        }

        public void SetConditionCompleted(IScenarioCondition condition)
        {
            if (ScenarioNode is not ConditionNode conditionNode)
                return;

            for (var i = 0; i < conditionNode.Components.Count; i++)
                if (conditionNode.Components[i].Equals(condition))
                    extensionContainer[i].AddToClassList(CompletedConditionUSS);
        }

        public override void OnSelected()
        {
            base.OnSelected();
            var nodeEditor = GraphEditorWindow.ConstructElementEditor();
            nodeEditor.BindNode(this);
            
            AddToClassList(SelectedNodeUSS);
            foreach (var linkedNodeView in GetLinkedNodeViews())
                linkedNodeView.AddToClassList(LinkedNodeUSS);
        }
        public override void OnUnselected()
        {
            base.OnUnselected();
            var nodeEditor = GraphEditorWindow.ConstructElementEditor();
            nodeEditor.UnbindElement();
            
            RemoveFromClassList(SelectedNodeUSS);
            foreach (var linkedNodeView in GetLinkedNodeViews())
                linkedNodeView.RemoveFromClassList(LinkedNodeUSS);
        }

        private List<ScenarioNodeView> GetLinkedNodeViews()
        {
            var list = new List<ScenarioNodeView>(1);
            switch (ScenarioNode)
            {
                case IPortInNode inNode:
                    if (inNode.OutputNode != null)
                        if (graphController.NodeViews.TryGetValue(inNode.OutputNode.Hash, out var outView))
                            list.Add(outView);
                    break;
                case IPortOutNode outNode:
                    if (outNode.InputNode != null)
                        if (graphController.NodeViews.TryGetValue(outNode.InputNode.Hash, out var inView))
                            list.Add(inView);
                    break;
            }
            return list;
        }

        public override void SetPosition(Rect newPos)
        {
            style.position = Position.Absolute;
            style.left = newPos.x;
            style.top = newPos.y;
        }
        public void SetCenterPosition(Vector2 newPos)
        {
            var rect = GetPosition();
            rect.center = newPos;
            SetPosition(rect);
        }
        public void SetOnlyPosition(Vector2 newPos)
        {
            var rect = GetPosition();
            rect.position = newPos;
            SetPosition(rect);
        }

        private void AddInputPort()
        {
            inputContainer.Clear();
            var inputPort = ScenarioPortView.Create<ScenarioLinkView>(Orientation.Horizontal, 
                Direction.Input, Port.Capacity.Multi, IScenarioLinkFlow.GetModelType);
            inputPort.portName = "prev";
            inputContainer.Add(inputPort);
        }
        private void AddOutputPort()
        {
            outputContainer.Clear();
            var outputPort = ScenarioPortView.Create<ScenarioLinkView>(Orientation.Horizontal,
                Direction.Output, Port.Capacity.Multi, IScenarioLinkFlow.GetModelType);
            outputPort.portName = "next";
            outputContainer.Add(outputPort);
        }

        private void Bind(IScenarioNode node)
        {
            ScenarioNode = node;
            UpdateVisuals();
        }

        public void UpdateVisuals()
        {
            UpdateUSS();
            SetTitleText(Content.GetName(ScenarioNode));
            SetItems(Content.GetItems(ScenarioNode, Context));
        }

        private string currentUSS = string.Empty;
        private void UpdateUSS()
        {
            titleContainer.RemoveFromClassList(currentUSS);
            currentUSS = HasNulls() ? NullUSS : Content.USSClass;
            titleContainer.AddToClassList(currentUSS);
        }

        private bool HasNulls()
        {
            return ScenarioNode switch
            {
                IScenarioNodeComponents componentsNode => // Action/Condition
                    HasNulls(componentsNode),
                ISubgraphNode subgraphNode => !subgraphNode.Json,
                IPortInNode portInNode => portInNode.OutputNode == null,
                IPortOutNode portOutNode => portOutNode.InputNode == null,
                _ => false
            };
        }

        private bool HasNulls(IEnumerable<IScenarioComponent> components)
        {
            var nodeOverrides = Context?.NodeOverrides.GetValueOrDefault(ScenarioNode.Hash);
            return nodeOverrides != null ? HasNullsVariables(components, nodeOverrides) : HasNullsStandard(components);
        }

        private bool HasNullsVariables(IEnumerable<IScenarioComponent> components, List<IComponentVariables> nodeOverrides)
        {
            using var componentsEnumerator = components.GetEnumerator();
            using var overridesEnumerator = nodeOverrides.GetEnumerator();
            
            while (componentsEnumerator.MoveNext())
            {
                overridesEnumerator.MoveNext();
                var component = componentsEnumerator.Current;
                var componentOverrides = overridesEnumerator.Current;
                if (component == null) continue;
                
                var members = component.GetComponentFields();
                foreach (var member in members)
                {
                    var value = component.GetValueByField(member) 
                                ?? componentOverrides?.GetValueOrDefault(member.Name);
                    var attribute = member.TryGetAttribute<ScenarioMetaAttribute>();
                    if (FieldIsNull(value, attribute)) return true;
                }
            }

            return false;
        }
        private bool HasNullsStandard(IEnumerable<IScenarioComponent> components)
        {
            foreach (var component in components)
            {
                if (component == null) continue;
                var members = component.GetComponentFields();
                foreach (var member in members)
                {
                    var value = component.GetValueByField(member);
                    var attribute = member.TryGetAttribute<ScenarioMetaAttribute>();
                    if (FieldIsNull(value, attribute)) return true;
                }
            }

            return false;
        }

        private static bool FieldIsNull(object value, [CanBeNull] ScenarioMetaAttribute meta)
        {
            switch (value)
            {
                case null:
                    return meta is not { CanBeNull: true };
                case Object unityObject when !unityObject:
                    return true;
            }

            return false;
        }

        private void SetTitleText(string text)
        {
            titleContainer.Remove(titleContainer.Q<Label>());
            var titleLabel = new Label
            {
                name = "titleLabel",
                text = text
            };
            titleContainer.Insert(0, titleLabel);
        }

        private void SetItems(IEnumerable<IEditorNodeContent.Item> items)
        {
            extensionContainer.Clear();
            foreach (var item in items)
            {
                var label = new Label
                {
                    text = item.Text,
                    style =
                    {
                        fontSize = new StyleLength(item.Font),
                    }
                };
                extensionContainer.Add(label);
            }
            RefreshExpandedState();
        }
    }
}