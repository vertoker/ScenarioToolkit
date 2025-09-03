using Scenario.Core.Model.Interfaces;

// Previous: ScenarioModelV2
//  Current: ScenarioModelV3
//     Next: ScenarioModelV4

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
    public class ScenarioModelV3 : IScenarioCompatibilityModel
    {
        public ScenarioContextV2 Context { get; set; } = new();
        public ScenarioGraphV1 Graph { get; set; } = new();
        public EditorGraphV3 EditorGraph { get; set; } = new();
    }
}