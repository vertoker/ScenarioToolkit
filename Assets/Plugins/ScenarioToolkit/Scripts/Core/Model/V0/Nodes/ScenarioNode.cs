using System.Threading.Tasks;
using Newtonsoft.Json;

// Previous: 
//  Current: ScenarioNode
//     Next: ScenarioNodeV1

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
    [JsonObject(IsReference = true)]
    public abstract class ScenarioNode
    {
        public string Name { get; set; } = string.Empty;

        public abstract ScenarioNodeV1 ConvertV1();
    }
}