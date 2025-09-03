using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using VRF.Players.Hands;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Actions
{
    [ScenarioMeta("Создаёт копию этих скинов и делает их активными на игроке", typeof(HandSkinsController))]
    public struct CreateNewHandSkins : IScenarioAction, IComponentDefaultValues
    {
        [ScenarioMeta("Может быть null", CanBeNull = true)]
        public HandSkin Left;
        [ScenarioMeta("Может быть null", CanBeNull = true)]
        public HandSkin Right;
        
        public void SetDefault()
        {
            Left = null;
            Right = null;
        }
    }
}