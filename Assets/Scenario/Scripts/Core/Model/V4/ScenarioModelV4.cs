using Scenario.Core.Model.Interfaces;

// Previous: ScenarioModelV3
//  Current: ScenarioModelV4
//     Next: ScenarioModelV5

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
    public class ScenarioModelV4 : IScenarioCompatibilityModel
    {
        public ScenarioContextV2 Context { get; set; } = new();
        public ScenarioGraphV1 Graph { get; set; } = new();
        public EditorGraphV3 EditorGraph { get; set; } = new();
    }
}