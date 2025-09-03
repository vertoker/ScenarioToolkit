using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using SimpleUI.Core;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Actions
{
    [ScenarioMeta("Устанавливает активность у экрана через Open/Close (всегда асинхронно)", typeof(ScreenBase))]
    public struct SetScreenActivity : IScenarioAction
    {
        public ScreenBase Screen;
        public bool Active;
    }
}