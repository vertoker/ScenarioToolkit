using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using VRF.BNG_Framework.Scripts.Core;
using VRF.Inventory.Scriptables;
using VRF.Scenario.Systems;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Actions.Items
{
    [ScenarioMeta("Удаляет/роняет Grabbable из пояса", typeof(PlayerBeltAddGrabbable))]
    public struct PlayerBeltRemoveGrabbable : IScenarioAction, IComponentDefaultValues
    {
        [ScenarioMeta("Идентификатор слота на поясе игрока")]
        public uint SlotID;
        [ScenarioMeta("Каким образом убрать объект из пояса")]
        public BeltRemoveType RemoveType;
        
        public void SetDefault()
        {
            SlotID = 0;
            RemoveType = BeltRemoveType.Destroy;
        }
    }
}