using Scenario.Base.Components.Conditions;
using Scenario.Core.Model;
using Scenario.Core.Model.Interfaces;
using Scenario.Utilities;
using ScenarioToolkit.Core.Player;
using ScenarioToolkit.Shared;
using ScenarioToolkit.Shared.Attributes;

// ReSharper disable once CheckNamespace
namespace Scenario.Base.Components.Actions
{
    [ScenarioMeta("Увеличивают СУЩЕСТВУЮЩУЮ переменную в ЛОКАЛЬНОМ контексте")]
    public struct VariableIncrement : IScenarioActionContext
    {
        public string VariableName;
        [ScenarioMeta(RequestStaticData.VariableIsValue)]
        public ObjectTyped Value;
        [ScenarioMeta(RequestStaticData.EnvironmentInfo)]
        public VariableEnvironmentType EnvironmentType;
        [ScenarioMeta("Отправить Condition с новой переменной")]
        public bool Notify;
        
        public void SetDefault()
        {
            VariableName = null;
            Value = TypesReflection.FallbackTypedValue;
            EnvironmentType = VariableEnvironmentType.NVE;
            Notify = true;
        }
        public IScenarioActionContextData GetRequestData()
            => new VariableIncrementContext { Source = this };
    }
    
    public struct VariableIncrementContext : IScenarioActionContextData
    {
        public VariableIncrement Source;
        public NodeVariablesContext Variables;

        public void Construct(NodeExecutionContext context)
        {
            Variables = context.Variables;
        }
    }
}