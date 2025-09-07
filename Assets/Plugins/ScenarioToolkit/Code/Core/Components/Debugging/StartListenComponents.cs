using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Core.Player;

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
    public struct StartListenComponents : IScenarioActionContext
    {
        public void SetDefault() { }
        
        public IScenarioActionContextData GetRequestData() 
            => new StartListenComponentsContext { Source = this };
    }
    
    public struct StartListenComponentsContext : IScenarioActionContextData
    {
        public StartListenComponents Source;
        public ScenarioPlayer Player;

        public StopListenComponentsContext GetStopContext()
        {
            return new StopListenComponentsContext 
                { Player = Player };
        }
        
        public void Construct(NodeExecutionContext context)
        {
            Player = context.Player;
        }
    }
}