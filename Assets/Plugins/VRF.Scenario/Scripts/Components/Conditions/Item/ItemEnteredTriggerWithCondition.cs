using Scenario.Core.Model.Interfaces;
using Scenario.Utilities;
using Scenario.Utilities.Attributes;
using VRF.Inventory.Scriptables;
using VRF.Scenario.Interfaces;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Systems
{
    [ScenarioMeta("Инвентарный предмет находится в TriggerZone (с условием триггером)", 
        typeof(TriggerSystem), typeof(ITriggerable))]
    public struct ItemEnteredTriggerWithCondition : IScenarioCondition
    {
        public TriggerZone TriggerZone;
        public InventoryItemConfig ItemType;
        public bool TriggerState;
    }
}