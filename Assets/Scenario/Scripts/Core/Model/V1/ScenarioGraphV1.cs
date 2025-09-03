using System;
using System.Collections.Generic;
using System.Linq;
using Scenario.Core.Model.Interfaces;
using UnityEngine;

// Previous: Graph
//  Current: ScenarioGraphV1
//     Next: ScenarioGraphV5

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
    public class ScenarioGraphV1
    {
        public HashSet<ScenarioLinkV1> Links { get; set; } = new();
        public HashSet<ScenarioNodeV1> Nodes { get; set; } = new();
        
        // Оставлено для работы разных алгоритмов совместимости
        
        public ScenarioLinkV1 AddLink(ScenarioLinkV1 link)
        {
            if (Links.Any(l => l.Equals(link)))
                throw new ArgumentException("Link already exists");
            Links.Add(link);
            return link;
        }
        public void AddNode(ScenarioNodeV1 scenarioNode)
        {
            Nodes.Add(scenarioNode);
        }
        
        public IEnumerable<ScenarioLinkV1> GetIncomingLinks(ScenarioNodeV1 scenarioNode) 
            => Links.Where(link => link.To == scenarioNode);
        public IEnumerable<ScenarioLinkV1> GetOutcomingLinks(ScenarioNodeV1 scenarioNode) 
            => Links.Where(link => link.From == scenarioNode);
        public IEnumerable<ScenarioNodeV1> GetIncomingNodes(ScenarioNodeV1 scenarioNode)
            => GetIncomingLinks(scenarioNode).Select(link => link.From);
        public IEnumerable<ScenarioNodeV1> GetOutcomingNodes(ScenarioNodeV1 scenarioNode)
            => GetOutcomingLinks(scenarioNode).Select(link => link.To);
    }
}