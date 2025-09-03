using Scenario.Core.Model.Interfaces;
using Newtonsoft.Json;

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
    public readonly struct NodeRef
    {
        public static readonly NodeRef Empty = new();
        
        [JsonProperty] public readonly int Hash;

        public NodeRef(IScenarioNode node)
        {
            Hash = node.Hash;
        }
        public NodeRef(int hash)
        {
            Hash = hash;
        }

        public bool IsEmpty() => Hash == 0;
    }
}