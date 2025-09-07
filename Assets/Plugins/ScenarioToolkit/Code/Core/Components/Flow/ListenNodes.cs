using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Core.Player;

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
    public struct ListenNodes : IScenarioActionContext
    {
        public bool ListenActivations;
        public bool ListenDeactivations;
        
        public void SetDefault()
        {
            ListenActivations = true;
            ListenDeactivations = true;
        }

        public IScenarioActionContextData GetRequestData() 
            => new ListenNodesContext { Source = this };
    }

    public struct ListenNodesContext : IScenarioActionContextData
    {
        public ListenNodes Source;
        public ScenarioPlayer Player;
        
        public void Construct(NodeExecutionContext context)
        {
            Player = context.Player;
        }
    }
}