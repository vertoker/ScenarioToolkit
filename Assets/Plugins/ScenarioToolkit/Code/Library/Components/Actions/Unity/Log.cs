using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Shared.Attributes;

// ReSharper disable once CheckNamespace
namespace Scenario.Base.Components.Actions
{
    [ScenarioMeta("Выводит текст в unity консоль")]
    public struct Log : IScenarioAction, IComponentDefaultValues
    {
        public string Message;
        public ScenarioLogType Type;
        
        public void SetDefault()
        {
            Message = null;
            Type = ScenarioLogType.Log;
        }
    }
    
    public enum ScenarioLogType
    {
        Log = 0,
        LogWarning = 1,
        LogError = 2,
    }
}