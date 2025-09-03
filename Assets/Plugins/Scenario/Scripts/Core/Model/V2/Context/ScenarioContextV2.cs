using System.Collections.Generic;
using Scenario.Core.Model.Interfaces;

// Previous: 
//  Current: ScenarioContextV2
//     Next: ScenarioContextV6

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
    public class ScenarioContextV2
    {
        public Dictionary<string, ObjectTyped> Variables { get; set; } = new();
        public Dictionary<int, List<ComponentVariablesV2>> NodeOverrides { get; set; } = new();
    }
}