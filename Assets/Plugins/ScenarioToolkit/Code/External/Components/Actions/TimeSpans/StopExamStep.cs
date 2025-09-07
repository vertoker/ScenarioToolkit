using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Core.Scriptables;
using ScenarioToolkit.Shared.Attributes;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Actions
{
    [ScenarioMeta("Заканчивает шаг экзамена", typeof(StopExamStep))]
    public struct StopExamStep : IScenarioAction, IComponentDefaultValues
    {
        public ExamStep Step;
        [ScenarioMeta("Условия пройденности, может быть пройден или нет")]
        public bool IsPassed;
        
        public void SetDefault()
        {
            Step = null;
            IsPassed = true;
        }
    }
}