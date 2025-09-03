using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Actions.Items
{
    public struct PlayerBeltDeleteSlot : IScenarioAction, IComponentDefaultValues
    {
        [ScenarioMeta("Идентификатор слота на поясе игрока")]
        public uint SlotID;

        public void SetDefault()
        {
            SlotID = 0;
        }
    }
}