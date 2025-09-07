using Scenario.Base.Components.Conditions;
using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Core.Player;
using ScenarioToolkit.Shared.Attributes;

// ReSharper disable once CheckNamespace
namespace Scenario.Base.Components.Actions
{
    [ScenarioMeta("Удаляет переменную в ЛОКАЛЬНОМ контексте")]
    public struct VariableRemove : IScenarioActionContext
    {
        public string VariableName;
        [ScenarioMeta(RequestStaticData.EnvironmentInfo)]
        public VariableEnvironmentType EnvironmentType;
        
        public void SetDefault()
        {
            VariableName = null;
            EnvironmentType = VariableEnvironmentType.NVE;
        }
        public IScenarioActionContextData GetRequestData()
            => new VariableRemoveContext { Source = this };
    }
    
    public struct VariableRemoveContext : IScenarioActionContextData
    {
        public VariableRemove Source;
        public NodeVariablesContext Variables;

        public void Construct(NodeExecutionContext context)
        {
            Variables = context.Variables;
        }
    }
}