using Scenario.Core.Model.Interfaces;
using Scenario.Core.Player;

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
    public struct StopListenComponents : IScenarioActionContext
    {
        public void SetDefault() { }
        
        public IScenarioActionContextData GetRequestData() 
            => new StopListenComponentsContext { Source = this };
    }
    
    public struct StopListenComponentsContext : IScenarioActionContextData
    {
        public StopListenComponents Source;
        public ScenarioPlayer Player;
        
        public void Construct(NodeExecutionContext context)
        {
            Player = context.Player;
        }
    }
}