using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using VRF.Scenario.Systems;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Actions
{
    [ScenarioMeta("Заканчивает экзамен", typeof(StartExam))]
    public struct StopExam : IScenarioAction, IComponentDefaultValues
    {
        [ScenarioMeta("Условия пройденности, может быть пройден или нет")]
        public bool IsExamPassed;
        
        public void SetDefault()
        {
            IsExamPassed = true;
        }
    }
}