using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using VRF.BNG_Framework.Scripts.Components;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Actions
{
    [ScenarioMeta("Запускает анимацию исчезновения чёрного экрана на игроке", typeof(FadeIn), typeof(ScreenFader))]
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