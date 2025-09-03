using Scenario.Core.Model.Interfaces;
using Scenario.Core.Scriptables;
using Scenario.Utilities.Attributes;
using VRF.Scenario.Systems;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Actions
{
    [ScenarioMeta("Начинает экзамен (запускает таймер)", typeof(StopExam), typeof(ExamSystem))]
    public struct StartExam : IScenarioAction, IComponentDefaultValues
    {
        public ScenarioModule Module;
        public int Seconds;
        
        public void SetDefault()
        {
            Module = null;
            Seconds = 600;
        }
    }
}