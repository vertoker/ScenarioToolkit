using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using ZLinq;
using ZLinq.Linq;

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model.Interfaces
{
    using GetEditorLinksType = ValueEnumerable<Select<FromHashSet<int>, int, IEditorLink>, IEditorLink>;
    using GetEditorNodesType = ValueEnumerable<Select<Select<FromHashSet<int>, 
        int, IEditorLink>, IEditorLink, IEditorNode>, IEditorNode>;
    using GetEditorGraphType = ValueEnumerable<Select<FromHashSet<int>, int, IEditorGroup>, IEditorGroup>;
    using GetAllLinksType = ValueEnumerable<Concat<Select<FromHashSet<int>, int, IEditorLink>, 
        Select<FromHashSet<int>, int, IEditorLink>, IEditorLink>, IEditorLink>;
    
    using NodesKeyType = ValueEnumerable<Select<FromDictionary<int, 
        IEditorNode>, KeyValuePair<int, IEditorNode>, int>, int>;
    using LinksKeyType = ValueEnumerable<Select<FromDictionary<int, 
        IEditorLink>, KeyValuePair<int, IEditorLink>, int>, int>;
    using GroupsKeyType = ValueEnumerable<Select<FromDictionary<int, 
        IEditorGroup>, KeyValuePair<int, IEditorGroup>, int>, int>;
    using NodesValuesType = ValueEnumerable<Select<FromDictionary<int, 
        IEditorNode>, KeyValuePair<int, IEditorNode>, IEditorNode>, IEditorNode>;
    using LinksValuesType = ValueEnumerable<Select<FromDictionary<int, 
        IEditorLink>, KeyValuePair<int, IEditorLink>, IEditorLink>, IEditorLink>;
    using GroupsValuesType = ValueEnumerable<Select<FromDictionary<int, 
        IEditorGroup>, KeyValuePair<int, IEditorGroup>, IEditorGroup>, IEditorGroup>;
    
    /// <summary>
    /// Дополнительный набор данных для IScenarioGraph с информацией для редактора
    /// </summary>
    public interface IEditorGraph : IModelReflection<EditorGraphV6, IEditorGraph>
    {
        public Dictionary<int, IEditorNode> Nodes { get; set; }
        public Dictionary<int, IEditorLink> Links { get; set; }
        public Dictionary<int, IEditorGroup> Groups { get; set; }
        
        public IEnumerable<int> NodesKeys => Nodes.Keys;
        public IEnumerable<int> LinksKeys => Links.Keys;
        public IEnumerable<int> GroupsKeys => Groups.Keys;
        public IEnumerable<IEditorNode> NodesValues => Nodes.Values;
        public IEnumerable<IEditorLink> LinksValues => Links.Values;
        public IEnumerable<IEditorGroup> GroupsValues => Groups.Values;
        
        public NodesKeyType NodesKeyAVE => Nodes.AsValueEnumerable().Select(p => p.Key);
        public LinksKeyType LinksKeyAVE => Links.AsValueEnumerable().Select(p => p.Key);
        public GroupsKeyType GroupsKeyAVE => Groups.AsValueEnumerable().Select(p => p.Key);
        public NodesValuesType NodesValuesAVE => Nodes.AsValueEnumerable().Select(p => p.Value);
        public LinksValuesType LinksValuesAVE => Links.AsValueEnumerable().Select(p => p.Value);
        public GroupsValuesType GroupsValuesAVE => Groups.AsValueEnumerable().Select(p => p.Value);
        
        public int NodesCount => Nodes.Count;
        public int LinksCount => Links.Count;
        public int GroupsCount => Groups.Count;
        
        public void Clear();
        
        // Nodes

        public bool AddNode([CanBeNull] IEditorNode editorNode);
        public bool AddNewNode([CanBeNull] IScenarioNode node, Vector2 position = new());
        
        public bool RemoveNode(int hashNode);
        public bool RemoveNode([CanBeNull] IScenarioNode node);
        public bool RemoveNode([CanBeNull] IEditorNode editorNode);
        
        public bool ContainsNode(int hashNode);
        public bool ContainsNode([CanBeNull] IScenarioNode node);
        public bool ContainsNode([CanBeNull] IEditorNode editorNode);

        public IEditorNode GetNode(int hashNode);
        public IEditorNode GetNode([CanBeNull] IScenarioNode node);
        
        public bool GetNode(int hashNode, out IEditorNode editorNode);
        public bool GetNode([CanBeNull] IScenarioNode node, out IEditorNode editorNode);
        
        // Nodes Utility
        
        public IEnumerable<IEditorLink> GetIncomingLinks(IEditorNode editorNode);
        public GetEditorLinksType GetIncomingLinksAVE(IEditorNode editorNode);
        public IEnumerable<IEditorLink> GetOutcomingLinks(IEditorNode editorNode);
        public GetEditorLinksType GetOutcomingLinksAVE(IEditorNode editorNode);

        public IEnumerable<IEditorNode> GetIncomingNodes(IEditorNode editorNode);
        public GetEditorNodesType GetIncomingNodesAVE(IEditorNode editorNode);
        public IEnumerable<IEditorNode> GetOutcomingNodes(IEditorNode editorNode);
        public GetEditorNodesType GetOutcomingNodesAVE(IEditorNode editorNode);

        public IEnumerable<IEditorLink> GetAllLinks(IEditorNode editorNode);
        public GetAllLinksType GetAllLinksAVE(IEditorNode editorNode);
        
        // Links
        
        public bool AddLink([CanBeNull] IEditorLink editorLink);
        public bool AddLinkWithNodes([CanBeNull] IEditorLink editorLink);

        public bool RemoveLink([CanBeNull] IEditorLink editorLink);
        public bool RemoveLinkWithNodes([CanBeNull] IEditorLink editorLink);
        public bool RemoveLink([CanBeNull] IEditorNode from, [CanBeNull] IEditorNode to);

        public IEditorLink GetLink(int linkHash);
        public bool GetNode(int linkHash, out IEditorLink link);
        
        public bool ContainsLink(int linkHash);
        public bool ContainsLink([CanBeNull] IEditorLink editorLink);
        public bool ContainsLink([CanBeNull] IEditorNode from, [CanBeNull] IEditorNode to);
        
        public bool AddNewLink([CanBeNull] IEditorNode from, [CanBeNull] IEditorNode to, out IEditorLink editorLink);
        public IEditorLink AddNewLink([CanBeNull] IEditorNode from, [CanBeNull] IEditorNode to);
        
        // Groups

        public bool AddGroup([CanBeNull] IEditorGroup editorGroup);
        public bool AddGroupWithNodes([CanBeNull] IEditorGroup editorGroup);
        
        public bool RemoveGroup([CanBeNull] IEditorGroup editorGroup);
        public bool RemoveGroupWithNodes([CanBeNull] IEditorGroup editorGroup);
        
        public bool ContainsGroup(int groupHash);
        public bool ContainsGroup([CanBeNull] IEditorGroup editorGroup);
        
        public IEditorGroup AddNewGroup([CanBeNull] IEnumerable<IEditorNode> editorNode, Vector2 position = new());
        public IEditorGroup AddNewGroup<TEnumerator>(ValueEnumerable<TEnumerator, IEditorNode> editorNodes,
            Vector2 position = new()) where TEnumerator : struct, IValueEnumerator<IEditorNode>;
        
        public bool AddNewGroup([CanBeNull] IEnumerable<IEditorNode> editorNodes, out IEditorGroup editorGroup);
        public bool AddNewGroup<TEnumerator>(ValueEnumerable<TEnumerator, IEditorNode> editorNodes,
            out IEditorGroup editorGroup) where TEnumerator : struct, IValueEnumerator<IEditorNode>;

        public IEnumerable<IEditorGroup> GetGroups(IEditorNode editorNode);
        public GetEditorGraphType GetGroupsAVE(IEditorNode editorNode);
        
        public void UpdateHash([CanBeNull] IEditorGroup group, int oldHash, int newHash);
    }
}