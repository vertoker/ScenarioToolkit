using System.Collections.Generic;
using System.Linq;
using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Shared.Extensions;
using UnityEngine;
using ZLinq;
using ZLinq.Linq;

// Previous: EditorGraphV5
//  Current: EditorGraphV6
//     Next: 

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
    using GetEditorLinksType = ValueEnumerable<Select<FromHashSet<int>, int, IEditorLink>, IEditorLink>;
    using GetEditorNodesType = ValueEnumerable<Select<Select<FromHashSet<int>, 
        int, IEditorLink>, IEditorLink, IEditorNode>, IEditorNode>;
    using GetEditorGraphType = ValueEnumerable<Select<FromHashSet<int>, int, IEditorGroup>, IEditorGroup>;
    using GetAllLinksType = ValueEnumerable<Concat<Select<FromHashSet<int>, int, IEditorLink>, 
        Select<FromHashSet<int>, int, IEditorLink>, IEditorLink>, IEditorLink>;

    public class EditorGraphV6 : IEditorGraph
    {
        public Dictionary<int, IEditorNode> Nodes { get; set; } = new();
        public Dictionary<int, IEditorLink> Links { get; set; } = new();
        public Dictionary<int, IEditorGroup> Groups { get; set; } = new();
        
        public void Clear()
        {
            Links.Clear();
            Nodes.Clear();
            Groups.Clear();
        }
        
        // Nodes
        
        public bool AddNode(IEditorNode editorNode)
        {
            if (editorNode == null) return false;
            
            if (Nodes.TryAdd(editorNode.Hash, editorNode))
            {
                editorNode.ClearAll();
                return true;
            }
            return false;
        }
        public bool AddNewNode(IScenarioNode node, Vector2 position = new())
        {
            if (node == null) return false;
            var editorNode = IEditorNode.CreateNew();
            editorNode.Node = node;
            editorNode.Position = position;
            return Nodes.TryAdd(editorNode.Hash, editorNode);
        }

        public bool RemoveNode(int hashNode) => RemoveNode(GetNode(hashNode));
        public bool RemoveNode(IScenarioNode node) => RemoveNode(GetNode(node));
        public bool RemoveNode(IEditorNode editorNode)
        {
            if (editorNode == null) return false;
            var result = Nodes.Remove(editorNode.Hash);
            if (!result) return false;

            foreach (var link in GetAllLinksAVE(editorNode))
                RemoveLink(link);
            foreach (var group in GetGroupsAVE(editorNode))
                RemoveGroup(group);
            return true;
        }

        public bool ContainsNode(int hashNode) => Nodes.ContainsKey(hashNode);
        public bool ContainsNode(IScenarioNode node) => node != null && Nodes.ContainsKey(node.Hash);
        public bool ContainsNode(IEditorNode editorNode) => editorNode != null && Nodes.ContainsKey(editorNode.Hash);

        public IEditorNode GetNode(int hashNode) => Nodes.GetValueOrDefault(hashNode);
        public IEditorNode GetNode(IScenarioNode node) => node == null ? null : Nodes.GetValueOrDefault(node.Hash);
        
        public bool GetNode(int hashNode, out IEditorNode editorNode) => Nodes.TryGetValue(hashNode, out editorNode);
        public bool GetNode(IScenarioNode node, out IEditorNode editorNode)
        {
            if (node == null)
            {
                editorNode = null;
                return false;
            }
            return Nodes.TryGetValue(node.Hash, out editorNode);
        }
        
        // Nodes Utility
        
        // Можно использовать GetValueOrDefault, но graph не предусматривает некорректные хэши
        public IEnumerable<IEditorLink> GetIncomingLinks(IEditorNode editorNode)
            => editorNode.IncomingLinks.Select(GetIncomingLinks_Select);
        public GetEditorLinksType GetIncomingLinksAVE(IEditorNode editorNode)
            => editorNode.IncomingLinks.AsValueEnumerable().Select(GetIncomingLinks_Select);
        private IEditorLink GetIncomingLinks_Select(int linkHash)
        {
            if (Links.TryGetValue(linkHash, out var link)) return link;
            Debug.LogError($"HashLink {linkHash} can't be founded");
            return null;
        }

        public IEnumerable<IEditorLink> GetOutcomingLinks(IEditorNode editorNode)
            => editorNode.OutcomingLinks.Select(GetOutcomingLinks_Select);
        public GetEditorLinksType GetOutcomingLinksAVE(IEditorNode editorNode)
            => editorNode.OutcomingLinks.AsValueEnumerable().Select(GetOutcomingLinks_Select);
        private IEditorLink GetOutcomingLinks_Select(int linkHash)
        {
            if (Links.TryGetValue(linkHash, out var link)) return link;
            Debug.LogError($"HashLink {linkHash} can't be founded");
            return null;
        }

        public IEnumerable<IEditorNode> GetIncomingNodes(IEditorNode editorNode)
            => GetIncomingLinks(editorNode).Select(GetIncomingNodes_Select);
        public GetEditorNodesType GetIncomingNodesAVE(IEditorNode editorNode)
            => GetIncomingLinksAVE(editorNode).Select(GetIncomingNodes_Select);
        private static IEditorNode GetIncomingNodes_Select(IEditorLink link)
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

        public IEnumerable<IEditorNode> GetOutcomingNodes(IEditorNode editorNode)
            => GetOutcomingLinks(editorNode).Select(GetOutcomingNodes_Select);
        public GetEditorNodesType GetOutcomingNodesAVE(IEditorNode editorNode)
            => GetOutcomingLinksAVE(editorNode).Select(GetOutcomingNodes_Select);
        private static IEditorNode GetOutcomingNodes_Select(IEditorLink link)
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

        public IEnumerable<IEditorLink> GetAllLinks(IEditorNode editorNode)
            => GetIncomingLinks(editorNode).Concat(GetOutcomingLinks(editorNode));
        public GetAllLinksType GetAllLinksAVE(IEditorNode editorNode)
            => GetIncomingLinksAVE(editorNode).Concat(GetOutcomingLinksAVE(editorNode));
        
        // Links

        public bool AddLink(IEditorLink editorLink)
        {
            if (editorLink?.From == null || editorLink.To == null) return false;

            if (Links.TryAdd(editorLink.Hash, editorLink))
            {
                editorLink.From?.OutcomingLinks.Add(editorLink.Hash);
                editorLink.To?.IncomingLinks.Add(editorLink.Hash);
                return true;
            }
            return false;
        }
        public bool AddLinkWithNodes(IEditorLink editorLink)
        {
            if (editorLink?.From == null || editorLink.To == null) return false;

            if (Links.TryAdd(editorLink.Hash, editorLink))
            {
                AddNode(editorLink.From);
                AddNode(editorLink.To);
                
                editorLink.From.OutcomingLinks.Add(editorLink.Hash);
                editorLink.To.IncomingLinks.Add(editorLink.Hash);
                return true;
            }
            return false;
        }

        public bool RemoveLink(IEditorLink editorLink)
        {
            if (editorLink == null) return false;

            if (Links.Remove(editorLink.Hash))
            {
                editorLink.From.OutcomingLinks.Remove(editorLink.Hash);
                editorLink.To.IncomingLinks.Remove(editorLink.Hash);
                return true;
            }
            return false;
        }
        public bool RemoveLinkWithNodes(IEditorLink editorLink)
        {
            if (editorLink == null) return false;

            if (Links.Remove(editorLink.Hash))
            {
                RemoveNode(editorLink.From);
                RemoveNode(editorLink.To);
                
                editorLink.From.OutcomingLinks.Remove(editorLink.Hash);
                editorLink.To.IncomingLinks.Remove(editorLink.Hash);
                return true;
            }
            return false;
        }
        public bool RemoveLink(IEditorNode from, IEditorNode to)
        {
            if (from == null || to == null) return false;
            var linkHash = IHashable.Combine(from, to);
            return RemoveLink(Links.GetValueOrDefault(linkHash));
        }
        
        public IEditorLink GetLink(int linkHash) => Links.GetValueOrDefault(linkHash);
        public bool GetNode(int linkHash, out IEditorLink link) => Links.TryGetValue(linkHash, out link);

        public bool ContainsLink(int linkHash) => Links.ContainsKey(linkHash);
        public bool ContainsLink(IEditorLink editorLink)
            => editorLink != null && Links.ContainsKey(editorLink.Hash);
        public bool ContainsLink(IEditorNode from, IEditorNode to)
        {
            if (from == null || to == null) return false;
            var linkHash = IHashable.Combine(from, to);
            return Links.ContainsKey(linkHash);
        }

        public bool AddNewLink(IEditorNode from, IEditorNode to, out IEditorLink editorLink)
        {
            editorLink = IEditorLink.CreateNew();
            editorLink.From = from; editorLink.To = to;
            return AddLinkWithNodes(editorLink);
        }
        public IEditorLink AddNewLink(IEditorNode from, IEditorNode to)
        {
            var flowLink = IEditorLink.CreateNew();
            flowLink.From = from; flowLink.To = to;
            AddLinkWithNodes(flowLink);
            return flowLink;
        }
        
        // Groups

        public bool AddGroup(IEditorGroup editorGroup)
        {
            if (editorGroup == null) return false;

            if (Groups.TryAdd(editorGroup.Hash, editorGroup))
            {
                foreach (var editorNode in editorGroup.Nodes)
                    editorNode.Groups.Add(editorGroup.Hash);
                return true;
            }
            return false;
        }
        public bool AddGroupWithNodes(IEditorGroup editorGroup)
        {
            if (editorGroup == null) return false;

            if (Groups.TryAdd(editorGroup.Hash, editorGroup))
            {
                foreach (var editorNode in editorGroup.Nodes)
                {
                    AddNode(editorNode);
                    editorNode.Groups.Add(editorGroup.Hash);
                }
                return true;
            }
            return false;
        }

        public bool RemoveGroup(IEditorGroup editorGroup)
        {
            if (editorGroup == null) return false;

            if (Groups.Remove(editorGroup.Hash))
            {
                foreach (var editorNode in editorGroup.Nodes)
                    editorNode.Groups.Remove(editorGroup.Hash);
                return true;
            }
            return false;
        }
        public bool RemoveGroupWithNodes(IEditorGroup editorGroup)
        {
            if (editorGroup == null) return false;

            if (Groups.Remove(editorGroup.Hash))
            {
                foreach (var editorNode in editorGroup.Nodes)
                {
                    RemoveNode(editorNode);
                    editorNode.Groups.Remove(editorGroup.Hash);
                }
                return true;
            }
            return false;
        }

        public bool ContainsGroup(int groupHash) => Groups.ContainsKey(groupHash);
        public bool ContainsGroup(IEditorGroup editorGroup) => editorGroup != null && Groups.ContainsKey(editorGroup.Hash);

        public IEditorGroup AddNewGroup(IEnumerable<IEditorNode> editorNodes, Vector2 position = new())
        {
            if (editorNodes == null) return null;
            
            var editorGroup = IEditorGroup.CreateNew();
            editorGroup.Nodes.AddRange(editorNodes);
            editorGroup.Position = position;
            
            AddGroupWithNodes(editorGroup);
            return editorGroup;
        }
        public IEditorGroup AddNewGroup<TEnumerator>(ValueEnumerable<TEnumerator, IEditorNode> editorNodes, Vector2 position = new())
            where TEnumerator : struct, IValueEnumerator<IEditorNode>
        {
            var editorGroup = IEditorGroup.CreateNew();
            editorGroup.Nodes = editorNodes.ToHashSet();
            editorGroup.Position = position;
            
            AddGroupWithNodes(editorGroup);
            return editorGroup;
        }
        
        public bool AddNewGroup(IEnumerable<IEditorNode> editorNodes, out IEditorGroup editorGroup)
        {
            if (editorNodes == null)
            {
                editorGroup = null;
                return false;
            }
            
            editorGroup = IEditorGroup.CreateNew();
            editorGroup.Nodes.AddRange(editorNodes);
            
            return AddGroupWithNodes(editorGroup);
        }
        public bool AddNewGroup<TEnumerator>(ValueEnumerable<TEnumerator, IEditorNode> editorNodes, out IEditorGroup editorGroup)
            where TEnumerator : struct, IValueEnumerator<IEditorNode>
        {
            editorGroup = IEditorGroup.CreateNew();
            editorGroup.Nodes.AddRange(editorNodes);
            editorGroup.Nodes = editorNodes.ToHashSet();
            
            return AddGroupWithNodes(editorGroup);
        }
        
        public IEnumerable<IEditorGroup> GetGroups(IEditorNode editorNode)
            => editorNode.Groups.Select(h => Groups[h]);
        public GetEditorGraphType GetGroupsAVE(IEditorNode editorNode)
            => editorNode.Groups.AsValueEnumerable().Select(h => Groups[h]);
        
        public void UpdateHash(IEditorGroup group, int oldHash, int newHash)
        {
            if (group == null) return;
            if (oldHash == newHash) return;
            
            // Нужно поменять Group.Hash и пересчитать хэши всех Group для Node

            group.Hash = newHash;
            Groups.Remove(oldHash);
            Groups.Add(newHash, group);
            
            foreach (var editorNode in group.Nodes)
            {
                editorNode.Groups.Remove(oldHash);
                editorNode.Groups.Add(newHash);
            }
        }
    }
}