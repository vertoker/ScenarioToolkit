using Scenario.Base.Components.Conditions;
using Scenario.Core.Model;
using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Core.Player;
using ScenarioToolkit.Shared.Attributes;

// ReSharper disable once CheckNamespace
namespace Scenario.Base.Components.Actions
{
    [ScenarioMeta("Посылает Condition VariableEquals в шину с текущей переменной из ЛОКАЛЬНОГО сценария")]
    public struct VariableNotify : IScenarioActionContext
    {
        public string VariableName;
        
        public void SetDefault()
        {
            VariableName = null;
        }
        public IScenarioActionContextData GetRequestData()
            => new VariableNotifyContext { Source = this };
    }
    
    public struct VariableNotifyContext : IScenarioActionContextData
    {
        public VariableNotify Source;
        public NodeVariablesContext Variables;

        public void Construct(NodeExecutionContext context)
        {
            Variables = context.Variables;
        }
    }
}