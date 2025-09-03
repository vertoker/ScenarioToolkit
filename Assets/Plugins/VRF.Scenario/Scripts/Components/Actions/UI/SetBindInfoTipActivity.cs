using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using VRF.Scenario.Services;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Actions
{
    [ScenarioMeta("Устанавливает активность кнопки бинда для экрана подсказки (на руке игрока)", typeof(PlayerScenarioScreensService))]
    public struct SetBindInfoTipActivity : IScenarioAction, IComponentDefaultValues
    {
        public bool Active;
        
        public void SetDefault()
        {
            Active = true;
        }
    }
}