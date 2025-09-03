using Scenario.Core.Model.Interfaces;
using Scenario.Core.Scriptables;
using Scenario.Utilities.Attributes;
using VRF.Scenario.Systems;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Actions
{
    [ScenarioMeta("Обновляет ОКОНЧЕННЫЙ шаг экзамена", typeof(UpdateExamStep), typeof(ExamSystem))]
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