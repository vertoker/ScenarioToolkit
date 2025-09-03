using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using VRF.BNG_Framework.Scripts.Core;
using VRF.Scenario.Systems;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Actions.Items
{
    [ScenarioMeta("Создаёт Grabbable на поясе", typeof(PlayerBeltRemoveGrabbable))]
    public struct PlayerBeltAddGrabbable : IScenarioAction, IComponentDefaultValues
    {
        [ScenarioMeta("Идентификатор слота на поясе игрока")]
        public uint SlotID;
        public Grabbable Grabbable;
        [ScenarioMeta("Каким образом убрать уже имеющийся предмет в слоте")]
        public BeltRemoveType OccupiedRemoveType;
        
        public void SetDefault()
        {
            SlotID = 0;
            Grabbable = null;
            OccupiedRemoveType = BeltRemoveType.Destroy;
        }

        public PlayerBeltRemoveGrabbable CastToRemove()
        {
            return new PlayerBeltRemoveGrabbable
            {
                SlotID = SlotID,
                RemoveType = OccupiedRemoveType,
            };
        }
    }
}