using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using VRF.Scenario.Installers;
using VRF.Scenario.Interfaces;
using VRF.Scenario.Models;
using VRF.Scenario.UI.ScenarioGame;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Actions
{
    [ScenarioMeta("Устанавливает PopupScreen перед лицом, Устанавливает данные в него и открывает через Open", typeof(PopupScreen))]
    public struct ShowPopupScreenAtLook : IScenarioAction, IShowAtLookSettingsProvider, IComponentDefaultValues
    {
        public PopupScreen Screen;
        
        [ScenarioMeta("Обязателен к установке", typeof(PopupScreen))]
        public string Title;
        [ScenarioMeta("Не обязателен к установке", typeof(PopupScreen))]
        public string Text;
        [ScenarioMeta("Не обязателен к установке", typeof(PopupScreen))]
        public string ButtonText;

        [ScenarioMeta("Перезаписывает стандартные параметры", typeof(PopupSystemInstaller))]
        public bool OverrideGlobalProperties;
        [ScenarioMeta("Дистанция перед лицом", typeof(ShowAtLookSettings))]
        public float DistanceSpawn;
        [ScenarioMeta("Scale у transform экрана при показе (DEPRECATED)", typeof(ShowAtLookSettings))]
        public float ScaleMultiplier;
        [ScenarioMeta("Уго поворота экрана при создании", typeof(ShowAtLookSettings))]
        public float OffsetAngle;

        public ShowAtLookSettings GetSettings()
            => new(DistanceSpawn, ScaleMultiplier, OffsetAngle);
        
        public void SetDefault()
        {
            Screen = null;

            Title = string.Empty;
            Text = string.Empty;
            ButtonText = string.Empty;
            
            OverrideGlobalProperties = false;
            DistanceSpawn = 2;
            ScaleMultiplier = 1;
            OffsetAngle = 0;
        }
    }
}