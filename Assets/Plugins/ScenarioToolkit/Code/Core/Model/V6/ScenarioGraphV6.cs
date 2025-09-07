using System;
using System.Collections.Generic;
using System.Linq;
using Scenario.Core.Model.Interfaces;
using UnityEngine;
using ZLinq;
using ZLinq.Linq;

// Previous: ScenarioGraphV5
//  Current: ScenarioGraphV6
//     Next: 

using GetLinksType = ZLinq.ValueEnumerable<ZLinq.Linq.Select<ZLinq.Linq.FromHashSet<int>, int, 
    Scenario.Core.Model.Interfaces.IScenarioLinkFlow>, Scenario.Core.Model.Interfaces.IScenarioLinkFlow>;
using GetNodesType = ZLinq.ValueEnumerable<ZLinq.Linq.Select<ZLinq.Linq.Select<ZLinq.Linq.FromHashSet<int>, 
        int, Scenario.Core.Model.Interfaces.IScenarioLinkFlow>, Scenario.Core.Model.Interfaces.IScenarioLinkFlow, 
    Scenario.Core.Model.Interfaces.IScenarioNodeFlow>, Scenario.Core.Model.Interfaces.IScenarioNodeFlow>;
using GetAllLinksType = ZLinq.ValueEnumerable<ZLinq.Linq.Concat<ZLinq.Linq.Select<ZLinq.Linq.FromHashSet<int>, 
        int, Scenario.Core.Model.Interfaces.IScenarioLinkFlow>, ZLinq.Linq.Select<ZLinq.Linq.FromHashSet<int>, 
        int, Scenario.Core.Model.Interfaces.IScenarioLinkFlow>, Scenario.Core.Model.Interfaces.IScenarioLinkFlow>, 
    Scenario.Core.Model.Interfaces.IScenarioLinkFlow>;

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
    public class ScenarioGraphV6 : IScenarioGraph
    {
        public Dictionary<int, IScenarioNode> Nodes { get; set; } = new();
        public Dictionary<int, IScenarioLinkFlow> Links { get; set; } = new();
        
        public void Clear()
        {
            Links.Clear();
            Nodes.Clear();
        }
        
        // Nodes

        public bool AddNode(IScenarioNode node)
        {
            if (node == null) return false;

            if (Nodes.TryAdd(node.Hash, node))
            {
                if (node is IScenarioNodeFlow flowNode)
                    flowNode.ClearAll();
                return true;
            }
            return false;
        }
        
        public bool RemoveNode(int hashNode) => RemoveNode(GetNode(hashNode));
        public bool RemoveNode(IScenarioNode node)
        {
            if (node == null) return false;
            var result = Nodes.Remove(node.Hash);
            if (!result) return false;

            if (node is IScenarioNodeFlow flowNode)
            {
                foreach (var link in GetAllLinksAVE(flowNode))
                    RemoveLink(link);
            }
            return true;
        }
        
        public bool ContainsNode(int hashNode) => Nodes.ContainsKey(hashNode);
        public bool ContainsNode(IScenarioNode node) => node != null && Nodes.ContainsKey(node.Hash);

        public IScenarioNode GetNode(int hashNode) => Nodes.GetValueOrDefault(hashNode);
        public IScenarioNodeFlow GetFlowNode(int hashNode) => Nodes.GetValueOrDefault(hashNode) as IScenarioNodeFlow;

        public bool GetNode(int hashNode, out IScenarioNode node) => Nodes.TryGetValue(hashNode, out node);
        public bool GetFlowNode(int hashNode, out IScenarioNodeFlow flowNode)
        {
            flowNode = Nodes.GetValueOrDefault(hashNode) as IScenarioNodeFlow;
            return flowNode == default(IScenarioNodeFlow);
        }
        
        // Nodes Utility
        
        // Можно использовать GetValueOrDefault, но graph не предусматривает некорректные хэши
        public IEnumerable<IScenarioLinkFlow> GetIncomingLinks(IScenarioNodeFlow flowNode)
            => flowNode.IncomingLinks.Select(GetIncomingLinks_Select);
        public GetLinksType GetIncomingLinksAVE(IScenarioNodeFlow flowNode)
            => flowNode.IncomingLinks.AsValueEnumerable().Select(GetIncomingLinks_Select);
        private IScenarioLinkFlow GetIncomingLinks_Select(int linkHash)
        {
            if (Links.TryGetValue(linkHash, out var link)) return link;
            Debug.LogError($"HashLink {linkHash} can't be founded");
            return null;
        }

        public IEnumerable<IScenarioLinkFlow> GetOutcomingLinks(IScenarioNodeFlow flowNode)
            => flowNode.OutcomingLinks.Select(GetOutcomingLinks_Select);
        public GetLinksType GetOutcomingLinksAVE(IScenarioNodeFlow flowNode)
            => flowNode.OutcomingLinks.AsValueEnumerable().Select(GetOutcomingLinks_Select);
        private IScenarioLinkFlow GetOutcomingLinks_Select(int linkHash)
        {
            if (Links.TryGetValue(linkHash, out var link)) return link;
            Debug.LogError($"HashLink {linkHash} can't be founded");
            return null;
        }

        public IEnumerable<IScenarioNodeFlow> GetIncomingNodes(IScenarioNodeFlow flowNode)
            => GetIncomingLinks(flowNode).Select(GetIncomingNodes_Select);
        public GetNodesType GetIncomingNodesAVE(IScenarioNodeFlow flowNode)
            => GetIncomingLinksAVE(flowNode).Select(GetIncomingNodes_Select);
        private static IScenarioNodeFlow GetIncomingNodes_Select(IScenarioLinkFlow link)
        {
            if (link == null)
            {
                Debug.LogError($"Link is null");
                return null;
            }
            if (link.From == null)
            {
                Debug.LogError($"Link.From is null");
                return null;
            }
            return link.From;
        }

        public IEnumerable<IScenarioNodeFlow> GetOutcomingNodes(IScenarioNodeFlow flowNode)
            => GetOutcomingLinks(flowNode).Select(GetOutcomingNodes_Select);
        public GetNodesType GetOutcomingNodesAVE(IScenarioNodeFlow flowNode)
            => GetOutcomingLinksAVE(flowNode).Select(GetOutcomingNodes_Select);
        private static IScenarioNodeFlow GetOutcomingNodes_Select(IScenarioLinkFlow link)
        {
            if (link == null)
            {
                Debug.LogError($"Link is null");
                return null;
            }
            if (link.To == null)
            {
                Debug.LogError($"Link.To is null");
                return null;
            }
            return link.To;
        }

        public IEnumerable<IScenarioLinkFlow> GetAllLinks(IScenarioNodeFlow flowNode)
            => GetIncomingLinks(flowNode).Concat(GetOutcomingLinks(flowNode));
        public GetAllLinksType GetAllLinksAVE(IScenarioNodeFlow flowNode)
            => GetIncomingLinksAVE(flowNode).Concat(GetOutcomingLinksAVE(flowNode));
        
        public void UpdateHash(IScenarioNode node, int oldHash, int newHash)
        {
            if (node == null) return;
            if (oldHash == newHash) return;
            
            // Нужно поменять Node.Hash, пересчитать хэши всех Link для Node
            // И изменить все ссылки на эти Link с обоих сторон

            node.Hash = newHash;
            Nodes.Remove(oldHash);
            Nodes.Add(newHash, node);

            if (node is IScenarioNodeFlow flowNode)
            {
                var incomingLinks = GetIncomingLinks(flowNode).ToArray();
                var outcomingLinks = GetOutcomingLinks(flowNode).ToArray();
                flowNode.IncomingLinks.Clear();
                flowNode.OutcomingLinks.Clear();
                
                foreach (var inLink in incomingLinks)
                {
                    var oldLinkHash = IHashable.Combine(inLink.From.Hash, oldHash);
                    Links.Remove(oldLinkHash);
                    Links.Add(inLink.Hash, inLink);
                    
                    inLink.From.OutcomingLinks.Remove(oldLinkHash);
                    inLink.From.OutcomingLinks.Add(inLink.Hash);
                    //flowNode.IncomingLinks.Remove(oldLinkHash); // inLink.To = flowNode
                    flowNode.IncomingLinks.Add(inLink.Hash);
                }
                foreach (var outLink in outcomingLinks)
                {
                    var oldLinkHash = IHashable.Combine(oldHash, outLink.To.Hash);
                    Links.Remove(oldLinkHash);
                    Links.Add(outLink.Hash, outLink);
                    
                    outLink.To.IncomingLinks.Remove(oldLinkHash);
                    outLink.To.IncomingLinks.Add(outLink.Hash);
                    //flowNode.OutcomingLinks.Remove(oldLinkHash); // outLink.From = flowNode
                    flowNode.OutcomingLinks.Add(outLink.Hash);
                }
            }
        }
        
        // Links

        public bool AddLink(IScenarioLinkFlow flowLink)
        {
            if (flowLink?.From == null || flowLink.To == null) return false;

            if (Links.TryAdd(flowLink.Hash, flowLink))
            {
                flowLink.From?.OutcomingLinks.Add(flowLink.Hash);
                flowLink.To?.IncomingLinks.Add(flowLink.Hash);
                return true;
            }
            return false;
        }
        public bool AddLinkWithNodes(IScenarioLinkFlow flowLink)
        {
            if (flowLink?.From == null || flowLink.To == null) return false;

            if (Links.TryAdd(flowLink.Hash, flowLink))
            {
                AddNode(flowLink.From);
                AddNode(flowLink.To);
                
                flowLink.From.OutcomingLinks.Add(flowLink.Hash);
                flowLink.To.IncomingLinks.Add(flowLink.Hash);
                return true;
            }
            return false;
        }

        public bool RemoveLink(IScenarioLinkFlow flowLink)
        {
            if (flowLink == null) return false;

            if (Links.Remove(flowLink.Hash))
            {
                flowLink.From.OutcomingLinks.Remove(flowLink.Hash);
                flowLink.To.IncomingLinks.Remove(flowLink.Hash);
                return true;
            }
            return false;
        }
        public bool RemoveLinkWithNodes(IScenarioLinkFlow flowLink)
        {
            if (flowLink == null) return false;

            if (Links.Remove(flowLink.Hash))
            {
                RemoveNode(flowLink.From);
                RemoveNode(flowLink.To);
                
                flowLink.From.OutcomingLinks.Remove(flowLink.Hash);
                flowLink.To.IncomingLinks.Remove(flowLink.Hash);
                return true;
            }
            return false;
        }
        public bool RemoveLink(IScenarioNodeFlow from, IScenarioNodeFlow to)
        {
            if (from == null || to == null) return false;
            var linkHash = IHashable.Combine(from, to);
            return RemoveLink(Links.GetValueOrDefault(linkHash));
        }
        
        public IScenarioLinkFlow GetLink(int linkHash) => Links.GetValueOrDefault(linkHash);
        public bool GetLink(int linkHash, out IScenarioLinkFlow link) => Links.TryGetValue(linkHash, out link);
        
        public bool ContainsLink(IScenarioLinkFlow flowLink)
            => flowLink != null && Links.ContainsKey(flowLink.Hash);
        public bool ContainsLink(IScenarioNodeFlow from, IScenarioNodeFlow to)
        {
            if (from == null || to == null) return false;
            var linkHash = IHashable.Combine(from, to);
            return Links.ContainsKey(linkHash);
        }

        public bool AddNewLink(IScenarioNodeFlow from, IScenarioNodeFlow to, out IScenarioLinkFlow flowLink)
        {
            flowLink = IScenarioLinkFlow.CreateNew();
            flowLink.From = from; flowLink.To = to;
            return AddLinkWithNodes(flowLink);
        }
        public IScenarioLinkFlow AddNewLink(IScenarioNodeFlow from, IScenarioNodeFlow to)
        {
            var flowLink = IScenarioLinkFlow.CreateNew();
            flowLink.From = from; flowLink.To = to;
            AddLinkWithNodes(flowLink);
            return flowLink;
        }
    }
}