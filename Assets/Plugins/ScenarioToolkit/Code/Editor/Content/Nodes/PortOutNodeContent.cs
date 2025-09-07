using System;
using System.Collections.Generic;
using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Core.Nodes;

namespace ScenarioToolkit.Editor.Content.Nodes
{
    public class PortOutNodeContent : IEditorNodeContent
    {
        public string TypeName => "Out";
        public string USSClass => "port";
        public Type NodeType => IPortOutNode.GetModelType;
        public Type InterfaceNodeType => typeof(IPortOutNode);
        public Type CompatibilityInterfaceNodeType => typeof(IScenarioCompatibilityPortOutNode);
        public bool HasInput => false;
        public bool HasOutput => true;
        
        public string GetName(IScenarioNode node)
            => string.IsNullOrWhiteSpace(node.Name) ? $" ({TypeName})" : $" {node.Name} ({TypeName})";
        public IEnumerable<IEditorNodeContent.Item> GetItems(IScenarioNode node, IScenarioContext context)
        {
            return Array.Empty<IEditorNodeContent.Item>();
        }
        
        public IScenarioNode CreateDefault() => IPortOutNode.CreateNew();
    }
}