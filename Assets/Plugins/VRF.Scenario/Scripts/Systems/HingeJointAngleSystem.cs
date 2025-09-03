using System.Collections.Generic;
using Scenario.Core;
using Scenario.Core.Systems;
using VRF.BNG_Framework.Scripts.Core;
using VRF.Scenario.Components.Conditions;
using Zenject;

namespace VRF.Scenario.Systems
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class HingeJointAngleSystem : BaseScenarioSystem
    {
        public HingeJointAngleSystem(SignalBus listener, IEnumerable<HingeHelper> hingeHelpers) : base(listener)
        {
            foreach (var helper in hingeHelpers)
                helper.onHingeChange.AddListener(a => AngleChanged(helper, a));
        }

        private void AngleChanged(HingeHelper helper, float angle)
        {
            Bus.Fire(new HingeJointAngleReached
            {
                HingeHelper = helper,
                Angle = (int)angle
            });
        }
    }
}