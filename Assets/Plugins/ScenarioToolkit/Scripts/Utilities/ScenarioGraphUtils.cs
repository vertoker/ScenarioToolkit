using System.Collections.Generic;
using System.Linq;
using Scenario.Core.Model;
using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Extensions;

namespace Scenario.Utilities
{
    public static class ScenarioGraphUtils
    {
        public static IEnumerable<NodeTopology<IScenarioNodeFlow>> BuildTopologyTree(this IScenarioGraph graph, IStartNode startNode,
            bool useInputs = false, bool useOutputs = true)
        {
            if (startNode == null)
                yield break;
            
            var visited = new HashSet<IScenarioNodeFlow>();
            var toVisit = new Queue<NodeTopology<IScenarioNodeFlow>>();
            
            toVisit.Enqueue(new NodeTopology<IScenarioNodeFlow>
            {
                Node = startNode,
                Depth = 0,
                Index = 0,
            });

            var currentDepth = 0;
            var currentIndex = 0;
            while (toVisit.Count > 0)
            {
                var bind = toVisit.Dequeue();

                if (bind.Depth > currentDepth)
                {
                    currentDepth = bind.Depth;
                    currentIndex = 0;
                }

                if (!visited.Add(bind.Node)) continue;
                yield return bind;
                
                var children = new HashSet<IScenarioNodeFlow>();

                if (useInputs) children.AddRange(graph.GetIncomingNodesAVE(bind.Node));
                if (useOutputs) children.AddRange(graph.GetOutcomingNodesAVE(bind.Node));

                foreach (var child in children)
                {
                    toVisit.Enqueue(new NodeTopology<IScenarioNodeFlow>
                    {
                        Node = child,
                        Depth = currentDepth + 1,
                        Index = currentIndex++,
                    });
                }
            }
        }
        
        public static IEnumerable<NodeTopology<ScenarioNodeV1>> BuildTopologyTree(this ScenarioGraphV1 graph, 
            ScenarioNodeV1 startNode, bool useInputs = false, bool useOutputs = true)
        {
            if (startNode == null)
                yield break;
            
            var visited = new HashSet<ScenarioNodeV1>();
            var toVisit = new Queue<NodeTopology<ScenarioNodeV1>>();
            
            toVisit.Enqueue(new NodeTopology<ScenarioNodeV1>
            {
                Node = startNode,
                Depth = 0,
                Index = 0,
            });

            var currentDepth = 0;
            var currentIndex = 0;
            while (toVisit.Count > 0)
            {
                var bind = toVisit.Dequeue();

                if (bind.Depth > currentDepth)
                {
                    currentDepth = bind.Depth;
                    currentIndex = 0;
                }

                if (!visited.Add(bind.Node)) continue;
                yield return bind;
                
                var children = new HashSet<ScenarioNodeV1>();

                if (useInputs) children.AddRange(graph.GetIncomingNodes(bind.Node));
                if (useOutputs) children.AddRange(graph.GetOutcomingNodes(bind.Node));

                foreach (var child in children)
                {
                    toVisit.Enqueue(new NodeTopology<ScenarioNodeV1>
                    {
                        Node = child,
                        Depth = currentDepth + 1,
                        Index = currentIndex++,
                    });
                }
            }
        }
        
        public struct NodeTopology<TNode>
        {
            public TNode Node;
            public int Depth;
            public int Index;
        }
    }
}