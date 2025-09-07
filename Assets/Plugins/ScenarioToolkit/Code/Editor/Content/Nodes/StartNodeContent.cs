using System;
using System.Collections.Generic;
using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Core.Nodes;

namespace ScenarioToolkit.Editor.Content.Nodes
{
    public class StartNodeContent : IEditorNodeContent
    {
        public string TypeName => "Start";
        public string USSClass => "start";
        public Type NodeType => IStartNode.GetModelType;
        public Type InterfaceNodeType => typeof(IStartNode);
        public Type CompatibilityInterfaceNodeType => typeof(IScenarioCompatibilityStartNode);
        public bool HasInput => false;
        public bool HasOutput => true;

        public string GetName(IScenarioNode node)
            => string.IsNullOrWhiteSpace(node.Name) ? $" ({TypeName})" : $" {node.Name} ({TypeName})";

        public IEnumerable<IEditorNodeContent.Item> GetItems(IScenarioNode node, IScenarioContext context)
        {
            return Array.Empty<IEditorNodeContent.Item>();
        }

        public IScenarioNode CreateDefault() => IStartNode.CreateNew();
    }
}