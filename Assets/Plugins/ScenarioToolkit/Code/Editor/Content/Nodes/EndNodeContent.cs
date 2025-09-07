using System;
using System.Collections.Generic;
using Scenario.Core.Model.Interfaces;
using Scenario.Core.Nodes;

namespace Scenario.Editor.Content.Nodes
{
    public class EndNodeContent : IEditorNodeContent
    {
        public string TypeName => "End";
        public string USSClass => "end";
        public Type NodeType => IEndNode.GetModelType;
        public Type InterfaceNodeType => typeof(IEndNode);
        public Type CompatibilityInterfaceNodeType => typeof(IScenarioCompatibilityEndNode);
        public bool HasInput => true;
        public bool HasOutput => false;

        public string GetName(IScenarioNode node)
            => string.IsNullOrWhiteSpace(node.Name) ? $" ({TypeName})" : $" {node.Name} ({TypeName})";
        
        public IEnumerable<IEditorNodeContent.Item> GetItems(IScenarioNode node, IScenarioContext context)
        {
            var endNode = (IEndNode)node;
            return new[]
            {
                new IEditorNodeContent.Item($"<b>End</b>: {(endNode.InstantEnd ? "true" : "false")}"),
            };
        }
        
        public IScenarioNode CreateDefault() => IEndNode.CreateNew();
    }
}