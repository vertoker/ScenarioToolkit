using System.Threading.Tasks;
using Scenario.Base.Components.Actions;
using Scenario.Core.Model.Interfaces;

// Previous: 
//  Current: EndNode
//     Next: EndNodeV1

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
    public class EndNode : ScenarioNode, IScenarioCompatibilityEndNode
    {
        public override ScenarioNodeV1 ConvertV1()
        {
            var node = new EndNodeV1 
                { Name = Name, };
            node.InitializeHash();
            return node;
        }
    }
}