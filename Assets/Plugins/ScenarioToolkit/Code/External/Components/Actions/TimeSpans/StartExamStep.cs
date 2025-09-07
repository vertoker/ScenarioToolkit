using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Core.Scriptables;
using ScenarioToolkit.Shared.Attributes;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Actions
{
    [ScenarioMeta("Запускает шаг экзамена", typeof(StopExamStep))]
    public struct StartExamStep : IScenarioAction, IComponentDefaultValues
    {
        public ExamStep Step;
        
        public void SetDefault()
        {
            Step = null;
        }
    }
}