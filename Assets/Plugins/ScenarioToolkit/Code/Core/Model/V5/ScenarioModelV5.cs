using Scenario.Core.Model.Interfaces;

// Previous: ScenarioModelV4
//  Current: ScenarioModelV5
//     Next: ScenarioModelV6

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
    public class ScenarioModelV5 : IScenarioCompatibilityModel
    {
        public ScenarioContextV2 Context { get; set; } = new();
        public ScenarioGraphV5 Graph { get; set; } = new();
        public EditorGraphV5 EditorGraph { get; set; } = new();
    }
}