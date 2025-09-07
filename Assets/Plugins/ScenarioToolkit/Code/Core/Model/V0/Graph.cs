using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

// Previous: 
//  Current: Graph
//     Next: ScenarioGraphV1

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
    public class Graph
    {
        [JsonProperty] private HashSet<Link> links = new();
        [JsonProperty] private HashSet<ScenarioNode> nodes = new();

        public IEnumerable<Link> Links => links;
        public IEnumerable<ScenarioNode> Nodes => nodes;

        
        // Функции были оставлены для совместимости со внешним конвертером из старой V0 в новую (эту) V0
        // Старая V0 - это прошлый ScenarioFramework, который ещё написал Вова Наумов в 2023
        
        
        public Link AddLink(Link link)
        {
            if (links.Any(l => l.Equals(link)))
                throw new ArgumentException("Link already exists");
            links.Add(link);
            return link;
        }
        public void RemoveLink(Link link)
        {
            links.Remove(link);
        }

        public Link AddLink(ScenarioNode from, ScenarioNode to)
        {
            var link = new Link
            {
                From = from,
                To = to
            };
            return AddLink(link);
        }
        public void RemoveLink(ScenarioNode from, ScenarioNode to)
        {
            var removeQuery = new Link
            {
                From = from,
                To = to
            };
            var link = links.First(link => link.Equals(removeQuery));
            RemoveLink(link);
        }

        public Link GetLink(ScenarioNode from, ScenarioNode to, bool createIfNotExists = false)
        {
            if (TryGetLink(from, to, out var link)) return link;
            return createIfNotExists ? AddLink(from, to) : throw new KeyNotFoundException();
        }
        public bool TryGetLink(ScenarioNode from, ScenarioNode to, out Link link)
        {
            link = links.FirstOrDefault(l => l.From == from && l.To == to);
            return link != null;
        }
        
        /// <summary> Добавляет новую ноду </summary>
        public void AddNode(ScenarioNode node)
        {
            nodes.Add(node);
        }
        /// <summary> Удаляет существующую ноду и все связи с ней </summary>
        public void RemoveNode(ScenarioNode node)
        {
            // Обязательно надо удалить все связи с этой нодой
            var adjacent = GetAdjacent(node).ToArray();
            foreach (var link in adjacent)
                RemoveLink(link);
            nodes.Remove(node);
        }
        
        /// <summary> Весь список связей, входящий в ноду </summary>
        public IEnumerable<Link> GetIncoming(ScenarioNode node) 
            => links.Where(link => link.To == node);
        /// <summary> Весь список связей, выходящий из ноды </summary>
        public IEnumerable<Link> GetOutcoming(ScenarioNode node) 
            => links.Where(link => link.From == node);
        /// <summary> Весь список связей, связанные с нодой </summary>
        public IEnumerable<Link> GetAdjacent(ScenarioNode node) 
            => links.Where(link => link.To == node || link.From == node);
        
        public ScenarioGraphV1 Convert()
        {
            var newGraph = new ScenarioGraphV1();

            var nodeDict = Nodes.ToDictionary(n => n, n => n.ConvertV1());
            
            foreach (var nodeV1 in nodeDict.Values)
                newGraph.AddNode(nodeV1);
            foreach (var link in links)
                newGraph.AddLink(link.ConvertV1(nodeDict));

            return newGraph;
        }
    }
}