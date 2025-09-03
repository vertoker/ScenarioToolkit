using System.Collections.Generic;
using Modules.VRF.Scenario.Components.Conditions;
using Scenario.Core;
using Scenario.Core.Systems;
using VRF.Scenario.Interfaces;
using Zenject;

namespace VRF.Scenario.Systems
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class FailExamSystem : BaseScenarioSystem
    {
        public FailExamSystem(SignalBus listener, IEnumerable<Failable> failables) : base(listener)
        {
            foreach (var failable in failables)
                failable.Failed += ExamFailed;
        }

        private void ExamFailed()
        {
            Bus.Fire(new ExamFailed());
        }
    }
}