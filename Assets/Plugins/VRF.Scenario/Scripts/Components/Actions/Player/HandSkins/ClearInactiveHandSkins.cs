using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using VRF.Players.Hands;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Actions
{
    [ScenarioMeta("Очищает неактивные руки у игрока", typeof(HandSkinsController))]
    public struct ClearInactiveHandSkins : IScenarioAction, IComponentDefaultValues
    {
        public bool Left;
        public bool Right;
        
        public void SetDefault()
        {
            Left = true;
            Right = true;
        }
    }
}