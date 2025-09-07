using System.Threading.Tasks;
using Scenario.Core.Model.Interfaces;

// Previous: 
//  Current: StartNode
//     Next: StartNodeV1

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
    public class StartNode : ScenarioNode, IScenarioCompatibilityStartNode
    {
        public override ScenarioNodeV1 ConvertV1()
        {
            var node = new StartNodeV1 
                { Name = Name, };
            node.InitializeHash();
            return node;
        }
    }
}