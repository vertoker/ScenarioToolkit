using Scenario.Core.Model.Interfaces;

// Previous: 
//  Current: ActionNode
//     Next: ActionNodeV1

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
    public class ActionNode : ComponentsNode<IScenarioAction>, IScenarioCompatibilityActionNode
    {
        public override ScenarioNodeV1 ConvertV1()
        {
            var node = new ActionNodeV1 
                { Name = Name, Components = Components, };
            node.InitializeHash();
            return node;
        }
    }
}