using System.Collections.Generic;
using System.Linq;
using Scenario.Core;
using Scenario.Core.Systems;
using VRF.BNG_Framework.Scripts.Core;
using VRF.Scenario.Components.Actions;
using Zenject;

namespace VRF.Scenario.Systems
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class GrabberSystem : BaseScenarioSystem
    {
        private readonly Grabber[] grabbers;

        public GrabberSystem(SignalBus listener, IEnumerable<Grabber> grabbers) :
            base(listener)
        {
            this.grabbers = grabbers.ToArray();
            listener.Subscribe<DropAllItems>(OnDropAll);
        }
        
        private void OnDropAll(DropAllItems _)
        {
            foreach (var grabber in grabbers)
                grabber.DidDrop();
        }
    }
}