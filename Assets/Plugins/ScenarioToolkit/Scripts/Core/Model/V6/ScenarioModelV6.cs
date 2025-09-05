using Scenario.Core.Model;
using Scenario.Core.Model.Interfaces;

// Previous: ScenarioModelV5
//  Current: ScenarioModelV6
//     Next: 

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
    public class ScenarioModelV6 : IScenarioModel
    {
        public IScenarioContext Context { get; set; } = new ScenarioContextV6();
        public IScenarioGraph Graph { get; set; } = new ScenarioGraphV6();
        public IEditorGraph EditorGraph { get; set; } = new EditorGraphV6();
    }
}