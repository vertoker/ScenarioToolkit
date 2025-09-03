using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using VRF.BNG_Framework.Scripts.Components;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Actions
{
    [ScenarioMeta("Запускает анимацию появления чёрного экрана на игроке", typeof(FadeOut), typeof(ScreenFader))]
    public struct FadeIn : IScenarioAction, IComponentDefaultValues
    {
        [ScenarioMeta("Модификатор скорости, реальное время по нему равно 1 / speed")]
        public float FadeInSpeed;
        
        public void SetDefault()
        {
            FadeInSpeed = 1;
        }
    }
}