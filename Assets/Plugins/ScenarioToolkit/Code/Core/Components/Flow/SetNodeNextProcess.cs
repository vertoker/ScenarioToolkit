using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Core.Player;

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
    public struct SetNodeNextProcess : IScenarioActionContext
    {
        public NodeRef Node;
        public bool NextProcess;
        
        public void SetDefault()
        {
            Node = NodeRef.Empty;
            NextProcess = false;
        }
        public IScenarioActionContextData GetRequestData()
            => new SetNodeNextProcessContext { Source = this };
    }
    
    public struct SetNodeNextProcessContext : IScenarioActionContextData
    {
        public SetNodeNextProcess Source;
        public ScenarioPlayer Player;
        
        public void Construct(NodeExecutionContext context)
        {
            Player = context.Player;
        }
    }
}