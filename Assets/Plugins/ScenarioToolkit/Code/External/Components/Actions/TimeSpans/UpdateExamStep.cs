using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Core.Scriptables;
using ScenarioToolkit.Shared.Attributes;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Actions
{
    [ScenarioMeta("Обновляет ОКОНЧЕННЫЙ шаг экзамена", typeof(UpdateExamStep))]
    public struct UpdateExamStep : IScenarioAction, IComponentDefaultValues
    {
        public ExamStep Step;
        [ScenarioMeta("Условия пройденности, может быть пройден или нет")]
        public bool IsPassed;
        public bool OverrideTime;
        
        public void SetDefault()
        {
            Step = null;
            IsPassed = false;
            OverrideTime = false;
        }
    }
}