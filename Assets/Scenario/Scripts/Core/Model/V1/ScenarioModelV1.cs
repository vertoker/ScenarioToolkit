using Scenario.Core.Model.Interfaces;

// Previous: ScenarioData
//  Current: ScenarioModelV1
//     Next: ScenarioModelV2

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
    public class ScenarioModelV1 : IScenarioCompatibilityModel
    {
        public ScenarioGraphV1 Graph { get; set; } = new();
        public EditorGraphV1 EditorGraph { get; set; } = new();
    }
}