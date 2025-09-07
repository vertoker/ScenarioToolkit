using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Core.World;
using ScenarioToolkit.Shared.Attributes;

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
    [ScenarioMeta("Запускает сценарий через Instance", 
        typeof(ScenarioLauncherInstance), typeof(SetScenarioInstance))]
    public struct StartScenarioInstance : IScenarioAction, IComponentDefaultValues
    {
        public ScenarioLauncherInstance Instance;
        public bool ForcePlay;

        public void SetDefault()
        {
            Instance = null;
            ForcePlay = false;
        }
    }
}