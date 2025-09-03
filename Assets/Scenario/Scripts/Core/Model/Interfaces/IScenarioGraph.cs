using System.Collections.Generic;
using JetBrains.Annotations;
using ZLinq;
using ZLinq.Linq;

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model.Interfaces
{
    using GetLinksType = ValueEnumerable<Select<FromHashSet<int>, int, IScenarioLinkFlow>, IScenarioLinkFlow>;
    using GetNodesType = ValueEnumerable<Select<Select<FromHashSet<int>, int, 
        IScenarioLinkFlow>, IScenarioLinkFlow, IScenarioNodeFlow>, IScenarioNodeFlow>;
    using GetAllLinksType = ValueEnumerable<Concat<Select<FromHashSet<int>, int, IScenarioLinkFlow>, 
        Select<FromHashSet<int>, int, IScenarioLinkFlow>, IScenarioLinkFlow>, IScenarioLinkFlow>;
    
    using NodesValuesType = ValueEnumerable<Select<FromDictionary<int, IScenarioNode>, 
        KeyValuePair<int, IScenarioNode>, IScenarioNode>, IScenarioNode>;
    using LinksValuesType =ValueEnumerable<Select<FromDictionary<int, IScenarioLinkFlow>, 
        KeyValuePair<int, IScenarioLinkFlow>, IScenarioLinkFlow>, IScenarioLinkFlow>;
    
    /// <summary>
    /// Главная модель проигрывания в runtime, является основой для всего player.
    /// Ядро сценария, определяющий поток и топологию сценария
    /// </summary>
    public interface IScenarioGraph : IModelReflection<ScenarioGraphV6, IScenarioGraph>
    {
        public Dictionary<int, IScenarioNode> Nodes { get; set; }
        public Dictionary<int, IScenarioLinkFlow> Links { get; set; }

        public IEnumerable<IScenarioNode> NodesValues => Nodes.Values;
        public IEnumerable<IScenarioLinkFlow> LinksValues => Links.Values;
        
        public NodesValuesType NodesValuesAVE => Nodes.AsValueEnumerable().Select(p => p.Value);
        public LinksValuesType LinksValuesAVE => Links.AsValueEnumerable().Select(p => p.Value);
        
        public int NodesCount => Nodes.Count;
        public int LinksCount => Links.Count;

        public void Clear();
        
        // Nodes
        
        public bool AddNode([CanBeNull] IScenarioNode node);
        
        public bool RemoveNode(int hashNode);
        public bool RemoveNode([CanBeNull] IScenarioNode node);
        
        public bool ContainsNode(int hashNode);
        public bool ContainsNode([CanBeNull] IScenarioNode node);
        
        public IScenarioNode GetNode(int hashNode);
        public IScenarioNodeFlow GetFlowNode(int hashNode);
        
        public bool GetNode(int hashNode, out IScenarioNode node);
        public bool GetFlowNode(int hashNode, out IScenarioNodeFlow flowNode);

        // Nodes Utility
        
        public IEnumerable<IScenarioLinkFlow> GetIncomingLinks(IScenarioNodeFlow flowNode);
        public GetLinksType GetIncomingLinksAVE(IScenarioNodeFlow flowNode);
        public IEnumerable<IScenarioLinkFlow> GetOutcomingLinks(IScenarioNodeFlow flowNode);
        public GetLinksType GetOutcomingLinksAVE(IScenarioNodeFlow flowNode);

        public IEnumerable<IScenarioNodeFlow> GetIncomingNodes(IScenarioNodeFlow flowNode);
        public GetNodesType GetIncomingNodesAVE(IScenarioNodeFlow flowNode);
        public IEnumerable<IScenarioNodeFlow> GetOutcomingNodes(IScenarioNodeFlow flowNode);
        public GetNodesType GetOutcomingNodesAVE(IScenarioNodeFlow flowNode);

        public IEnumerable<IScenarioLinkFlow> GetAllLinks(IScenarioNodeFlow flowNode);
        public GetAllLinksType GetAllLinksAVE(IScenarioNodeFlow flowNode);
        
        public void UpdateHash([CanBeNull] IScenarioNode node, int oldHash, int newHash);
        
        // Links
        
        public bool AddLink([CanBeNull] IScenarioLinkFlow flowLink);
        public bool AddLinkWithNodes([CanBeNull] IScenarioLinkFlow flowLink);
        
        public bool RemoveLink([CanBeNull] IScenarioLinkFlow flowLink);
        public bool RemoveLinkWithNodes([CanBeNull] IScenarioLinkFlow flowLink);
        public bool RemoveLink([CanBeNull] IScenarioNodeFlow from, [CanBeNull] IScenarioNodeFlow to);
        
        public IScenarioLinkFlow GetLink(int linkHash);
        public bool GetLink(int linkHash, out IScenarioLinkFlow link);

        public bool ContainsLink([CanBeNull] IScenarioLinkFlow flowLink);
        public bool ContainsLink([CanBeNull] IScenarioNodeFlow from, [CanBeNull] IScenarioNodeFlow to);
        
        public IScenarioLinkFlow AddNewLink([CanBeNull] IScenarioNodeFlow from, [CanBeNull] IScenarioNodeFlow to);
        public bool AddNewLink([CanBeNull] IScenarioNodeFlow from, [CanBeNull] IScenarioNodeFlow to, out IScenarioLinkFlow flowLink);
    }
}