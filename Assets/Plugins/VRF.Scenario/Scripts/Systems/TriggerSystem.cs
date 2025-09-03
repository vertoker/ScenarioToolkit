using System.Collections.Generic;
using Scenario.Core;
using Scenario.Core.Systems;
using Scenario.Utilities;
using VRF.BNG_Framework.Scripts.Core;
using VRF.Components.Items.Views;
using VRF.Components.Players.Views.Player;
using VRF.Scenario.Components.Conditions;
using VRF.Scenario.Interfaces;
using Zenject;

namespace VRF.Scenario.Systems
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class TriggerSystem : BaseScenarioSystem
    {
        public TriggerSystem(SignalBus listener, IEnumerable<TriggerZone> zones) : base(listener)
        {
            foreach (var zone in zones)
                zone.OnEntered += go =>
                {
                    Bus.Fire(new GameObjectEnteredTrigger()
                    {
                        GameObject = go,
                        TriggerZone = zone,
                    });
                    
                    if (go.TryGetComponent(out Grabbable grabbable))
                    {
                        Bus.Fire(new GrabbableEnteredTrigger
                        {
                            Grabbable = grabbable,
                            TriggerZone = zone,
                        });
                    }   
                    
                    if (go.TryGetComponent(out ItemView itemGo))
                    {
                        Bus.Fire(new ItemEnteredTrigger
                        {
                            ItemType = itemGo.ItemConfig,
                            TriggerZone = zone
                        });

                        if (go.TryGetComponent<ITriggerable>(out var triggerable))
                        {
                            Bus.Fire(new ItemEnteredTriggerWithCondition
                            {
                                TriggerZone = zone,
                                ItemType = itemGo.ItemConfig,
                                TriggerState = triggerable.TriggerState
                            });
                        }
                    }

                    if (go.TryGetComponent(out BasePlayerView _) || go.TryGetComponent(out BNGPlayerController _))
                        Bus.Fire(new PlayerEnteredTrigger
                        {
                            TriggerZone = zone
                        });
                };
        }
    }
}