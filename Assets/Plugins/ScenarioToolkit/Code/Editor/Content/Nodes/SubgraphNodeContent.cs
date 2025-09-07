using System;
using System.Collections.Generic;
using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Core.Nodes;
using ScenarioToolkit.Editor.Utilities;

namespace ScenarioToolkit.Editor.Content.Nodes
{
    public class SubgraphNodeContent : IEditorNodeContent
    {
        public string TypeName => "Subgraph";
        public string USSClass => "subgraph";
        public Type NodeType => ISubgraphNode.GetModelType;
        public Type InterfaceNodeType => typeof(ISubgraphNode);
        public Type CompatibilityInterfaceNodeType => typeof(IScenarioCompatibilitySubgraphNode);
        public bool HasInput => true;
        public bool HasOutput => true;

        public string GetName(IScenarioNode node)
            => string.IsNullOrWhiteSpace(node.Name) ? " (Subgraph)" : $" {node.Name}";
        
        public IEnumerable<IEditorNodeContent.Item> GetItems(IScenarioNode node, IScenarioContext context)
        {
            var subgraphNode = (ISubgraphNode)node;
            var json = subgraphNode.Json;
            return new[]
            {
                new IEditorNodeContent.Item($"<b>Json</b>: {(json ? json.name : null)}"),
                new IEditorNodeContent.Item(subgraphNode.ToGraphString()),
            };
        }

        public IScenarioNode CreateDefault() => ISubgraphNode.CreateNew();
    }
}