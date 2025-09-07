using Scenario.Core.DataSource;
using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Core.Scriptables;
using ScenarioToolkit.Shared.Attributes;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Actions
{
    [ScenarioMeta("Загружает модуль сценария с определёнными параметрами", typeof(ScenarioModules))]
    public struct LoadModule : IScenarioAction, IComponentDefaultValues
    {
        public ScenarioModule Module;
        public bool StopCurrentScenario;
        public bool ForceReloadScene;
        
        public void SetDefault()
        {
            Module = null;
            StopCurrentScenario = true;
            ForceReloadScene = true;
        }
    }
}