using Scenario.Core.Model.Interfaces;
using Scenario.Core.Scriptables;
using Scenario.Utilities.Attributes;
using VRF.Scenario.Systems;

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