using System;
using Modules.VRF.Scenario.Components.Conditions;
using Scenario.Core;
using Scenario.Core.Installers.Systems;
using Scenario.Core.Scriptables;
using Scenario.Core.Systems;
using Scenario.Utilities;
using UnityEngine;
using VRF.Scenario.Components.Actions;
using VRF.Scenario.Components.Conditions;
using Zenject;

namespace VRF.Scenario.Systems
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class ExamSystem : BaseScenarioSystem
    {
        public event Action<StartExam> ExamStarted;
        public event Action<TimerUpdate> ExamUpdated;
        public event Action<bool> ExamStopped;
        
        public event Action<ExamStep> ExamStepStarted;
        public event Action<ExamStep, bool> ExamStepStopped;
        public event Action<ExamStep, bool, bool> ExamStepUpdated;
        
        private readonly IDebugParam source;
        private readonly TimerSystem timerSystem;

        private float examTime;
        private string examName;
        private bool examPassed;
        private bool examStarted;

        public ExamSystem(SignalBus bus, IDebugParam source, TimerSystem timerSystem) : base(bus)
        {
            this.source = source;
            this.timerSystem = timerSystem;
            
            bus.Subscribe<StartExam>(StartExam);
            bus.Subscribe<TimerUpdate>(UpdateExam);
            bus.Subscribe<StopExam>(ExamEnded);
            bus.Subscribe<TimerCancelled>(ExamFailed);
            bus.Subscribe<TimerEnded>(ExamCancelled);
            
            bus.Subscribe<StartExamStep>(StartExamStep);
            bus.Subscribe<StopExamStep>(StopExamStep);
            bus.Subscribe<UpdateExamStep>(UpdateExamStep);
        }

        private void StartExam(StartExam component)
        {
            if (AssertLog.NotNull<StartExam>(component.Module, nameof(component.Module))) return;
            if (AssertLog.Above<StartExam>(component.Seconds, 0, nameof(component.Module))) return;

            if (examStarted)
            {
                Debug.LogWarning($"Exam {examName} is already started, drop");
                return;
            }
            examStarted = true;
            
            examName = $"exam-{component.Module.ModuleIdentifier}";
            examTime = component.Seconds;
            examPassed = false;
            
            var data = new StartTimer
            {
                InGameTime = component.Seconds, 
                RealTime = component.Seconds,
                ID = examName, View = null,
                EnableOnStart = true,
                DisableOnEnd = true,
            };
            
            ExamStarted?.Invoke(component);
            timerSystem.StartTimer(data);
            
            if (source.Debug)
                Debug.Log($"Exam <b>started</b>: {component.Module.ModuleName} ");
        }
        
        private void UpdateExam(TimerUpdate component)
        {
            if (component.TimerID != examName) return;
            component.Time = examTime - component.Time;
            ExamUpdated?.Invoke(component);
        }

        private void ExamEnded(StopExam component)
        {
            if (!examStarted)
            {
                Debug.LogWarning("Exam is not started, drop");
                return;
            }
            examStarted = false;
            
            examPassed = component.IsExamPassed;
            timerSystem.StopTimer(examName);
        }
        private void ExamFailed(TimerCancelled component)
        {
            if (component.ID != examName) return;
            EndExam();
        }
        private void ExamCancelled(TimerEnded component)
        {
            if (component.ID != examName) return;
            EndExam();
        }
        
        private void EndExam()
        {
            ExamStopped?.Invoke(examPassed);

            if (examPassed)
            {
                if (source.Debug)
                    Debug.Log("Exam <b>completed</b>: " + examName);
                Bus.Fire(new ExamCompleted());
            }
            else
            {
                if (source.Debug)
                    Debug.Log("Exam <b>failed</b>: " + examName);
                Bus.Fire(new ExamFailed());
            }
        }

        private void StartExamStep(StartExamStep component)
        {
            if (AssertLog.NotNull<StartExamStep>(component.Step, nameof(component.Step))) return;
            ExamStepStarted?.Invoke(component.Step);
        }
        private void StopExamStep(StopExamStep component)
        {
            if (AssertLog.NotNull<StopExamStep>(component.Step, nameof(component.Step))) return;
            ExamStepStopped?.Invoke(component.Step, component.IsPassed);
        }
        private void UpdateExamStep(UpdateExamStep component)
        {
            if (AssertLog.NotNull<UpdateExamStep>(component.Step, nameof(component.Step))) return;
            ExamStepUpdated?.Invoke(component.Step, component.IsPassed, component.OverrideTime);
        }
    }
}