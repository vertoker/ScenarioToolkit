using System;
using System.Diagnostics;
using Scenario.Core;
using Scenario.Core.Installers.Systems;
using Scenario.Core.Systems;
using VRF.Scenario.Components.Actions;
using Zenject;
using Debug = UnityEngine.Debug;

namespace VRF.Scenario.Systems
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class StopwatchSystem : BaseScenarioSystem
    {
        private readonly IDebugParam param;
        private readonly Stopwatch stopwatch;
        
        public event Action<TimeSpan> OnStopwatchStopped;
        
        public StopwatchSystem(SignalBus bus, IDebugParam param) : base(bus)
        {
            this.param = param;
            bus.Subscribe<StartStopwatch>(StartStopwatch);
            bus.Subscribe<StopStopwatch>(StopStopwatch);

            stopwatch = new Stopwatch();
        }

        private void StartStopwatch()
        {
            stopwatch.Reset();
            stopwatch.Start();
        }

        private void StopStopwatch()
        {
            if (!stopwatch.IsRunning)
                return;
            
            stopwatch.Stop();
            
            OnStopwatchStopped?.Invoke(stopwatch.Elapsed);
            if (param.Debug)
                Debug.Log($"<b>Stopwatch</b>: time elapsed={stopwatch.Elapsed}");
        }
    }
}