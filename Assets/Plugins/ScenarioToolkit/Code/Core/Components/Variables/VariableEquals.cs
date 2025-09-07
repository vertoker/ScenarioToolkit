using Scenario.Core.Model;
using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Core.Player;
using ScenarioToolkit.Shared;
using ScenarioToolkit.Shared.Attributes;

// ReSharper disable once CheckNamespace
namespace Scenario.Base.Components.Conditions
{
    [ScenarioMeta(RequestStaticData.VariableIsOverall, typeof(IScenarioConditionRequest))]
    public struct VariableEquals : IScenarioConditionRequest
    {
        public string VariableName;
        [ScenarioMeta(RequestStaticData.VariableIsValue)]
        public ObjectTyped Value;
        
        public void SetDefault()
        {
            VariableName = null;
            Value = ObjectTyped.ConstructNotNull(TypesReflection.SerializationNullType);
        }
        public IScenarioRequestData GetRequestData()
            => new RequestVariableEquals { Source = this };
    }

    public struct RequestVariableEquals : IScenarioRequestData
    {
        public VariableEquals Source;
        public NodeVariablesContext Variables;

        public void Construct(NodeExecutionContext context)
        {
            Variables = context.Variables;
        }
    }
    
    public static class RequestStaticData
    {
        public const string VariableIsOverall = "Само-вызываемый condition, обычный if, проверяет если переменная равна Value";
        public const string VariableIsValue = "С каким значением будет сравнение";
        public const string EnvironmentInfo = "LVE - значит переменная БУДЕТ передаваться доречним сценариям, NVE - значит НЕ БУДЕТ";
    }
}