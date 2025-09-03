using System.Collections.Generic;
using Scenario.Core.Systems.States;
using VRF.Scenario.Components.Actions;

namespace VRF.Scenario.States
{
    public class TimerState : IState
    {
        public Dictionary<string, PausedTimerData> PausedTimers = new();
        public Dictionary<string, RunningTimerData> RunningTimers = new();
        
        public class PausedTimerData
        {
            public StartTimer Component;
            public float PauseRealTime;
            
            public PausedTimerData(StartTimer component, float pauseRealTime)
            {
                Component = component;
                PauseRealTime = pauseRealTime;
            }
        }
        
        public class RunningTimerData : NetworkContinuousData
        {
            public StartTimer Component;
            public float LastPauseRealTime;
            
            public RunningTimerData(StartTimer component, float lastPauseRealTime, float seconds) : base(seconds)
            {
                Component = component;
                LastPauseRealTime = lastPauseRealTime;
            }
        }

        public void Clear()
        {
            PausedTimers.Clear();
            RunningTimers.Clear();
        }
    }
}