using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Core.World;
using ScenarioToolkit.Shared.Attributes;

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
    [ScenarioMeta("Останавливает сценарий, запущенный через Instance", typeof(ScenarioLauncherInstance))]
    public struct StopScenarioInstance : IScenarioAction, IComponentDefaultValues
    {
        public ScenarioLauncherInstance Instance;
        
        public void SetDefault()
        {
            Instance = null;
        }
    }
}