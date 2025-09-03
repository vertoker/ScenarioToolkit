using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using VRF.BNG_Framework.Scripts.Core;
using VRF.Inventory.Scriptables;
using VRF.Scenario.Systems;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Conditions
{
    [ScenarioMeta("Инвентарный предмет вошёл в SnapZone (не путать с GrabbableSnapped)", 
        typeof(ItemUnsnapped), typeof(GrabbableSnapped), typeof(SnapZone))]
    public struct ItemSnapped : IScenarioCondition
    {
        public SnapZone Zone;
        public InventoryItemConfig InventoryItemType;
    }
}