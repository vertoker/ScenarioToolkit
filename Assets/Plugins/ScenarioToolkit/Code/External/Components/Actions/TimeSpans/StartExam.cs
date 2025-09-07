using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Core.Scriptables;
using ScenarioToolkit.Shared.Attributes;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Actions
{
    [ScenarioMeta("Начинает экзамен (запускает таймер)", typeof(StopExam))]
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