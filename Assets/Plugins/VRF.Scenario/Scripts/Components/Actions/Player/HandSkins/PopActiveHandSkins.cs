using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using VRF.Players.Hands;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Actions
{
    [ScenarioMeta("Удаляет активные скины на руках, меняя их на предыдущие", typeof(HandSkinsController))]
    public struct PopActiveHandSkins : IScenarioAction, IComponentDefaultValues
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