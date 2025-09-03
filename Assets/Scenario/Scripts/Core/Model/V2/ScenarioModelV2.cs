using Scenario.Core.Model.Interfaces;

// Previous: ScenarioModelV1
//  Current: ScenarioModelV2
//     Next: ScenarioModelV3

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
    public class ScenarioModelV2 : IScenarioCompatibilityModel
    {
        public ScenarioContextV2 Context { get; set; } = new();
        public ScenarioGraphV1 Graph { get; set; } = new();
        public EditorGraphV1 EditorGraph { get; set; } = new();
    }
}