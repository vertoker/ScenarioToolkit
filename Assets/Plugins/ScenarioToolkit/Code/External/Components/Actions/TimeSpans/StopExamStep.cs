using Scenario.Core.Model.Interfaces;
using Scenario.Core.Scriptables;
using Scenario.Utilities.Attributes;
using VRF.Scenario.Systems;

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