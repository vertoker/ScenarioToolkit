using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.External.Systems;
using ScenarioToolkit.Shared.Attributes;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Actions
{
    [ScenarioMeta("Останавливает таймер по ID", typeof(ResumeTimer), typeof(TimerSystem))]
    public struct PauseTimer : IScenarioAction, IComponentDefaultValues
    {
        [ScenarioMeta("Идентификатор таймера, должен быть уникальным в рамках промежутках проигрывания")]
        public string ID;
        
        public void SetDefault()
        {
            ID = string.Empty;
        }
    }
}