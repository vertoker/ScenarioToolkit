using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Shared.Attributes;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Actions
{
    [ScenarioMeta("Загружает сцену (нужно использовать vrf конфиг)")]
    public struct LoadScene : IScenarioAction, IComponentDefaultValues
    {
        [ScenarioMeta("Может быть как name, так и path (Assets/...) сцены")]
        public string Scene;
        
        public void SetDefault()
        {
            Scene = string.Empty;
        }
    }
}