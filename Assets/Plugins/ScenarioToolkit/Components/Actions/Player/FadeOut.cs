using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Actions
{
    [ScenarioMeta("Запускает анимацию исчезновения чёрного экрана на игроке")]
    public struct FadeOut : IScenarioAction, IComponentDefaultValues
    {
        [ScenarioMeta("Модификатор скорости, реальное время по нему равно 1 / speed")]
        public float FadeOutSpeed;
        
        public void SetDefault()
        {
            FadeOutSpeed = 1;
        }
    }
}