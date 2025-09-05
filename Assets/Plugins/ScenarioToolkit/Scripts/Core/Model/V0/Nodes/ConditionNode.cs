using Scenario.Core.Model.Interfaces;

// Previous: 
//  Current: ConditionNode
//     Next: ConditionNodeV1

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
    public class ConditionNode : ComponentsNode<IScenarioCondition>, IScenarioCompatibilityConditionNode
    {
        public override ScenarioNodeV1 ConvertV1()
        {
            var node = new ConditionNodeV1 
                { Name = Name, Components = Components, };
            node.InitializeHash();
            return node;
        }
    }
}