using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using SimpleUI.Core;
using VRF.Scenario.Installers;
using VRF.Scenario.Interfaces;
using VRF.Scenario.Models;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Actions
{
    [ScenarioMeta("Устанавливает экран перед лицом и открывает через Open", typeof(ScreenBase))]
    public struct ShowScreenAtLook : IScenarioAction, IShowAtLookSettingsProvider, IComponentDefaultValues
    {
        public ScreenBase Screen;
        
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
            
            OverrideGlobalProperties = false;
            DistanceSpawn = 2;
            ScaleMultiplier = 1;
            OffsetAngle = 0;
        }
    }
}