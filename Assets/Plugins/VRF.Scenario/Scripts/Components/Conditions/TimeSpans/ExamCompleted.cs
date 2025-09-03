using Modules.VRF.Scenario.Components.Conditions;
using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using VRF.Scenario.Systems;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Conditions
{
    [ScenarioMeta("Вызывается если экзамен был выполнен (StopExam:true)", typeof(ExamFailed), typeof(ExamSystem))]
    public struct ExamCompleted : IScenarioCondition
    {
        
    }
}