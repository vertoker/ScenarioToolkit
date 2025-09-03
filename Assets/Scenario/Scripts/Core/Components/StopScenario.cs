using Scenario.Base.Components.Conditions;
using Scenario.Core.Model.Interfaces;
using Scenario.Core.Player;
using Scenario.Utilities.Attributes;

// ReSharper disable once CheckNamespace
namespace Scenario.Base.Components.Actions
{
    public struct StopScenario : IScenarioActionContext
    {
        public void SetDefault()
        {
            
        }
        public IScenarioActionContextData GetRequestData()
        {
            return new StopScenarioContext { Source = this };
        }
    }
    
    public struct StopScenarioContext : IScenarioActionContextData
    {
        public StopScenario Source;
        public ScenarioPlayer Player;

        public void Construct(NodeExecutionContext context)
        {
            Player = context.Player;
        }
    }
}