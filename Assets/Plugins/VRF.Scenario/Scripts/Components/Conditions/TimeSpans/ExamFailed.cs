using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using VRF.Scenario.Components.Conditions;
using VRF.Scenario.Systems;

// ReSharper disable once CheckNamespace
namespace Modules.VRF.Scenario.Components.Conditions
{
    [ScenarioMeta("Вызывается если экзамен был провален (StopExam:false) или вышло время (ExamSystem) или игрок сделал " +
                  "неправильное действие (FailExamSystem)", typeof(ExamCompleted), typeof(ExamSystem))]
    public struct ExamFailed : IScenarioCondition
    {
        
    }
}