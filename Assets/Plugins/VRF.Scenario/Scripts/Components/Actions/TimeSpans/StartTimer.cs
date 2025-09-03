using Scenario.Base.Components.Actions;
using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using VRF.Scenario.Systems;
using VRF.Scenario.UI.Game;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Actions
{
    [ScenarioMeta("Начинает таймер по ID", typeof(StopTimer), typeof(TimerSystem), typeof(StartDelayTrigger))]
    public struct StartTimer : IScenarioAction, IComponentDefaultValues
    {
        [ScenarioMeta("Идентификатор таймера, должен быть уникальным в рамках промежутках проигрывания")]
        public string ID;
        // Таймер имеет разное представление об внутри игровом времени и реальном времени
        [ScenarioMeta("Внутриигровое время, за которое пройдёт таймер")]
        public float InGameTime;
        [ScenarioMeta("Реальное время, за которое пройдёт таймер")]
        public float RealTime;
        
        [ScenarioMeta("Включает View на старте", typeof(TimerScreen))]
        public bool EnableOnStart;
        [ScenarioMeta("Выключает View на окончании", typeof(TimerScreen))]
        public bool DisableOnEnd;
        
        [ScenarioMeta("Отображение таймера на специальном экране (может быть null)", 
            RelatedComponents = new [] { typeof(TimerScreen) }, CanBeNull = true)]
        public TimerScreen View;
        
        public void SetDefault()
        {
            ID = null;
            View = null;

            InGameTime = 10;
            RealTime = 10;
            
            EnableOnStart = true;
            DisableOnEnd = true;
        }
    }
}