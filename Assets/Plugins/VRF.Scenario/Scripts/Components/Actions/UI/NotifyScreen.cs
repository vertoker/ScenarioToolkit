using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using SimpleUI.Core;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Actions
{
    [ScenarioMeta("Комплексный шорткат, который открывает экран, ждёт и закрывает экран", typeof(SetScreenActivity))]
    public struct NotifyScreen : IScenarioAction, IComponentDefaultValues
    {
        public ScreenBase Screen;
        public float Time;
        
        public void SetDefault()
        {
            Screen = null;
            Time = 1;
        }
    }
}