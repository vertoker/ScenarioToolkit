using System.Collections.Generic;
using Scenario.Core;
using Scenario.Core.Systems;
using VRF.BNG_Framework.Scripts.Core;
using VRF.Components.Items.Views;
using VRF.Scenario.Components.Conditions;
using VRF.Scenario.Components.Conditions.Item;
using Zenject;

namespace VRF.Scenario.Systems
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class SnappingSystem : BaseScenarioSystem
    {
        public SnappingSystem(SignalBus listener, IEnumerable<SnapZone> zones) : base(listener)
        {
            foreach (var zone in zones)
            {
                zone.OnSnapEvent.AddListener(g => Snapped(zone, g));
                zone.OnDetachEvent.AddListener(g => Unsnapped(zone, g));
            }
        }

        private void Snapped(SnapZone zone, Grabbable grabbable)
        {
            // TODO активируете когда появятся подписчики
            // TODO а ещё вызывает warning на старте так как вск SnapZone на старте вызывают этот ивент

            Bus.Fire(new GrabbableSnapped()
            {
                Zone = zone,
                Grabbable = grabbable
            });

            if (!grabbable.TryGetComponent(out ItemView grabbedItemType))
                return;

            Bus.Fire(new ItemSnapped
            {
                Zone = zone,
                InventoryItemType = grabbedItemType.ItemConfig
            });
        }

        private void Unsnapped(SnapZone zone, Grabbable grabbable)
        {
            Bus.Fire(new GrabbableUnsnapped()
            {
                Zone = zone,
                Grabbable = grabbable
            });

            if (!grabbable.TryGetComponent(out ItemView grabbedItemType))
                return;

            Bus.Fire(new ItemUnsnapped
            {
                Zone = zone,
                InventoryItemType = grabbedItemType.ItemConfig
            });
        }
    }
}