using System;
using System.Collections.Generic;
using System.Linq;
using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Core.Nodes;
using ScenarioToolkit.Shared;

namespace ScenarioToolkit.Editor.Content.Nodes
{
    public abstract class ComponentsNodeContent : IEditorNodeContent
    {
        public abstract string TypeName { get; }
        public abstract string USSClass { get; }
        public abstract Type NodeType { get; }
        public abstract Type InterfaceNodeType { get; }
        public abstract Type CompatibilityInterfaceNodeType { get; }
        public bool HasInput => true;
        public bool HasOutput => true;
        
        public string GetName(IScenarioNode node)
            => string.IsNullOrWhiteSpace(node.Name) ? $" ({TypeName})" : $" {node.Name}";
        
        public IEnumerable<IEditorNodeContent.Item> GetItems(IScenarioNode node, IScenarioContext context)
        {
            var componentsNode = (IScenarioNodeComponents)node;
            if (context?.NodeOverrides == null || !context.NodeOverrides.TryGetValue(node.Hash, out var nodeOverrides))
                return componentsNode.Select(c => c.ToGraphString()).Select(t => new IEditorNodeContent.Item(t));
            return GetItems(componentsNode, nodeOverrides);
        }
        private static IEnumerable<IEditorNodeContent.Item> GetItems(IScenarioNodeComponents node, IList<IComponentVariables> nodeOverrides)
        {
            using var componentsEnumerator = node.GetEnumerator();
            using var overridesEnumerator = nodeOverrides.GetEnumerator();
            
            while (componentsEnumerator.MoveNext())
            {
                overridesEnumerator.MoveNext();
                var component = componentsEnumerator.Current;
                var componentOverrides = overridesEnumerator.Current;
                var text = component.ToGraphString(componentOverrides);
                yield return new IEditorNodeContent.Item(text);
            }
        }
        public abstract IScenarioNode CreateDefault();
    }
}