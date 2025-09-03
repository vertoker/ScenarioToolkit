using System.Collections.Generic;
using Scenario.Core;
using Scenario.Core.Systems;
using VRF.BNG_Framework.Scripts.Helpers;
using VRF.Scenario.Components.Conditions;
using Zenject;

namespace VRF.Scenario.Systems
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class TeleportDestinationSystem : BaseScenarioSystem
    {
        public TeleportDestinationSystem(SignalBus listener, IEnumerable<TeleportDestination> destinations) :
            base(listener)
        {
            foreach (var destination in destinations)
                destination.OnPlayerTeleported.AddListener(() => OnPlayerTeleported(destination));
        }

        private void OnPlayerTeleported(TeleportDestination destination)
        {
            Bus.Fire(new PlayerTeleported { Destination = destination });
        }
    }
}