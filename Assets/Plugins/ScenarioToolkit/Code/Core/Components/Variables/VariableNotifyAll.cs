using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Core.Player;
using ScenarioToolkit.Shared.Attributes;

// ReSharper disable once CheckNamespace
namespace Scenario.Base.Components.Actions
{
    [ScenarioMeta("Посылает Condition'ы VariableEquals в шину на каждую переменную в ЛОКАЛЬНОМ сценарии")]
    public struct VariableNotifyAll : IScenarioActionContext
    {
        public void SetDefault()
        {
            
        }
        public IScenarioActionContextData GetRequestData()
            => new VariableNotifyAllContext { /*Source = this*/ };
    }
    
    public struct VariableNotifyAllContext : IScenarioActionContextData
    {
        //public VariableNotifyAll Source;
        public NodeVariablesContext Variables;

        public void Construct(NodeExecutionContext context)
        {
            Variables = context.Variables;
        }
    }
}