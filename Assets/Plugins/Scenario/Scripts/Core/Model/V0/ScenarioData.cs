using Scenario.Core.Model.Interfaces;

// Previous: 
//  Current: ScenarioData
//     Next: ScenarioModelV1

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
    /// <summary> (Legacy) Runtime модель до введения версий в модель </summary>
    public class ScenarioData : IScenarioCompatibilityModel
    {
        public Graph Graph = new();
        public string Name = string.Empty;
    }
}