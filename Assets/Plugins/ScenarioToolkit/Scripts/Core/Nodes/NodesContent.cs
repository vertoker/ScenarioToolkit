using System;
using System.Collections.Generic;
using System.Linq;
using Scenario.Core.Model.Interfaces;
using Scenario.Utilities;

namespace Scenario.Core.Nodes
{
    public static class NodesContent
    {
        private static readonly Dictionary<Type, IEditorNodeContent> NodeBinds;
        // ReSharper disable once InconsistentNaming
        private static readonly Dictionary<Type, IEditorNodeContent> INodeBinds;
        // ReSharper disable once InconsistentNaming
        private static readonly Dictionary<Type, IEditorNodeContent> ICompatibilityNodeBinds;
        
        public static IReadOnlyDictionary<Type, IEditorNodeContent> ReadOnlyNodeBinds => NodeBinds;
        public static IReadOnlyDictionary<Type, IEditorNodeContent> ReadOnlyINodeBinds => INodeBinds;
        public static IReadOnlyDictionary<Type, IEditorNodeContent> ReadOnlyICompatibilityNodeBinds => ICompatibilityNodeBinds;
        
        static NodesContent()
        {
            var elements = Reflection.GetImplementations<IEditorNodeContent>()
                .Select(Activator.CreateInstance).Cast<IEditorNodeContent>().ToArray();

            NodeBinds = elements.ToDictionary(e => e.NodeType, e => e);
            INodeBinds = elements.ToDictionary(e => e.InterfaceNodeType, e => e);
            ICompatibilityNodeBinds = elements.ToDictionary(e => e.CompatibilityInterfaceNodeType, e => e);

            /*NodeBinds = new Dictionary<Type, IEditorNodeContent>();
            INodeBinds = new Dictionary<Type, IEditorNodeContent>();

            foreach (var element in elements)
            {
                foreach (var nodeType in element.NodeTypes)
                    NodeBinds.Add(nodeType, element);
                foreach (var interfaceNodeType in element.InterfaceNodeTypes)
                    INodeBinds.Add(interfaceNodeType, element);
            }*/
        }

        public static IEditorNodeContent GetContent(this IEditorNode editorNode)
        {
            var type = editorNode.Node.GetType();
            var content = ReadOnlyNodeBinds[type];
            return content;
        }
        public static IEditorNodeContent GetContent(this IScenarioNode scenarioNode)
        {
            var type = scenarioNode.GetType();
            var content = ReadOnlyNodeBinds[type];
            return content;
        }
        public static IEditorNodeContent GetCompatibilityContent(this IScenarioCompatibilityNode scenarioNode)
        {
            //var type = scenarioNode.GetType();
            // усложнение алгоритмический сложности во имя работоспособности
            var pair = ReadOnlyICompatibilityNodeBinds.First(p => p.Key.IsInstanceOfType(scenarioNode));
            //var content = ReadOnlyICompatibilityNodeBinds[type];
            return pair.Value;
        }
        
        public static IEditorNodeContent GetContent<TInterfaceNode>()
            where TInterfaceNode : IScenarioNode
        {
            var type = typeof(TInterfaceNode);
            var content = ReadOnlyINodeBinds[type];
            return content;
        }
        public static IEditorNodeContent GetCompatibilityContent<TInterfaceNode>()
            where TInterfaceNode : IScenarioCompatibilityNode
        {
            var type = typeof(TInterfaceNode);
            var content = ReadOnlyICompatibilityNodeBinds[type];
            return content;
        }
    }
}