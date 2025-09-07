using Scenario.Core.Model.Interfaces;
using UnityEngine;

// Previous: 
//  Current: SubgraphNode
//     Next: SubgraphNodeV1

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
    public class SubgraphNode : ScenarioNode, IScenarioCompatibilitySubgraphNode
    {
        public TextAsset Json;
        
        public override ScenarioNodeV1 ConvertV1()
        {
            var node = new SubgraphNodeV1 
                { Name = Name, Json = Json, };
            node.InitializeHash();
            return node;
        }
    }
}