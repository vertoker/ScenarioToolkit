using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using VRF.Scenario.Systems;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Actions
{
    [ScenarioMeta("Останавливает и удаляет таймер по ID", typeof(StartTimer), typeof(TimerSystem))]
    public struct StopTimer : IScenarioAction, IComponentDefaultValues
    {
        [ScenarioMeta("Идентификатор таймера, должен быть уникальным в рамках промежутках проигрывания")]
        public string ID;
        
        public void SetDefault()
        {
            ID = string.Empty;
        }
    }
}