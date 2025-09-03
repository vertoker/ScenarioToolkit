using Scenario.Core.Model.Interfaces;
using Scenario.Core.Player;

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
    public struct SetNodeActivity : IScenarioActionContext
    {
        public NodeRef Node;
        public bool Active;
        public SetNodeActivityIf SetIf;
        
        public void SetDefault()
        {
            Node = NodeRef.Empty;
            Active = false;
            SetIf = SetNodeActivityIf.NotSpecified;
        }
        public IScenarioActionContextData GetRequestData()
            => new SetNodeActivityContext { Source = this };
    }

    public enum SetNodeActivityIf
    {
        NotSpecified = 0,
        OnlyIfEnabled = 1,
        OnlyIfDisabled = 2,
    }
    
    public struct SetNodeActivityContext : IScenarioActionContextData
    {
        public SetNodeActivity Source;
        public ScenarioPlayer Player;
        
        public void Construct(NodeExecutionContext context)
        {
            Player = context.Player;
        }
    }
}