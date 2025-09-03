using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Actions.Items
{
    public struct PlayerBeltInsertSlot : IScenarioAction, IComponentDefaultValues
    {
        [ScenarioMeta("Идентификатор слота на поясе игрока")]
        public uint SlotID;
        [ScenarioMeta("Высота центра слота относительно центра игрока")]
        public float Height;
        [ScenarioMeta("Дистанция относительно центра слота (не игрока)")]
        public float Distance;
        [ScenarioMeta("Угловая позиция на круге от forward игрока, в градусах")]
        public float Angle;
        
        public void SetDefault()
        {
            SlotID = 0;
            Height = 0;
            Distance = 0.2f;
            Angle = 0;
        }
    }
}