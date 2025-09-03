using System.Collections.Generic;
using Scenario.Core.Model.Interfaces;

// Previous: ScenarioContextV2
//  Current: ScenarioContextV6
//     Next: 

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
    public class ScenarioContextV6 : IScenarioContext
    {
        public Dictionary<string, ObjectTyped> Variables { get; set; } = new();
        public Dictionary<int, List<IComponentVariables>> NodeOverrides { get; set; } = new();
    }
}