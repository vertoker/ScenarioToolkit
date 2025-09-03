using System;
using System.Collections.Generic;
using Scenario.Core.Model;
using Scenario.Core.Model.Interfaces;
using Scenario.Core.Nodes;

namespace Scenario.Editor.Content.Nodes
{
    public class PortInNodeContent : IEditorNodeContent
    {
        public string TypeName => "In";
        public string USSClass => "port";
        public Type NodeType => IPortInNode.GetModelType;
        public Type InterfaceNodeType => typeof(IPortInNode);
        public Type CompatibilityInterfaceNodeType => typeof(IScenarioCompatibilityPortInNode);
        public bool HasInput => true;
        public bool HasOutput => false;
        
        public string GetName(IScenarioNode node)
            => string.IsNullOrWhiteSpace(node.Name) ? $" ({TypeName})" : $" {node.Name} ({TypeName})";
        
        public IEnumerable<IEditorNodeContent.Item> GetItems(IScenarioNode node, IScenarioContext context)
        {
            var portInNode = (IPortInNode)node;
            var text = $"<b>{Enum.GetName(typeof(ActivationType), portInNode.ActivationType)}</b>";
            return new[] { new IEditorNodeContent.Item(text) };
        }
        
        public IScenarioNode CreateDefault() => IPortInNode.CreateNew();
    }
}