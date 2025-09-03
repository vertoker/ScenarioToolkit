using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using VRF.Scenario.Systems;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Actions
{
    /// <summary> Продолжить исполнение таймера по ID </summary>
    [ScenarioMeta("Продолжает исполнение таймера по ID", typeof(PauseTimer), typeof(TimerSystem))]
    public struct ResumeTimer : IScenarioAction, IComponentDefaultValues
    {
        [ScenarioMeta("Идентификатор таймера, должен быть уникальным в рамках промежутках проигрывания")]
        public string ID;
        
        public void SetDefault()
        {
            ID = string.Empty;
        }
    }
}