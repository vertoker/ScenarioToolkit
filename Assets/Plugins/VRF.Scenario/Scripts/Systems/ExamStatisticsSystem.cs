using System;
using Scenario.Core;
using Scenario.Core.Installers.Systems;
using Scenario.Core.Scriptables;
using Scenario.Core.Systems;
using Scenario.Core.Systems.States;
using Scenario.Utilities;
using UnityEngine;
using VRF.DataSources.LocalCache;
using VRF.Scenario.Components.Actions;
using VRF.Scenario.Models;
using Zenject;

namespace VRF.Scenario.Systems
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class ExamStatisticsSystem : BaseScenarioSystem, IDisposable, IScenarioStateProvider//, IInitializable
    {
        private readonly ExamSystem examSystem;
        private readonly LocalCacheDataSource dataSource;
        private readonly IDebugParam source;

        public event Action ExamStarted;
        public event Action ExamStopped;
        public event Action ExamStepStarted;
        public event Action ExamStepStopped;
        public event Action ExamStepUpdated;

        public ExamStatisticsModel Model { get; private set; } = new();

        public ExamStatisticsSystem(ExamSystem examSystem, LocalCacheDataSource dataSource, 
            IDebugParam source, SignalBus bus) : base(bus)
        {
            this.examSystem = examSystem;
            this.dataSource = dataSource;
            this.source = source;
            Initialize();
        }
        
        public void Initialize()
        {
            examSystem.ExamStarted += OnExamStarted;
            examSystem.ExamStopped += OnExamStopped;
            examSystem.ExamStepStarted += OnExamStepStarted;
            examSystem.ExamStepStopped += OnExamStepStopped;
            examSystem.ExamStepUpdated += OnExamStepUpdated;
        }
        public void Dispose()
        {
            examSystem.ExamStarted -= OnExamStarted;
            examSystem.ExamStopped -= OnExamStopped;
            examSystem.ExamStepStarted -= OnExamStepStarted;
            examSystem.ExamStepStopped -= OnExamStepStopped;
            examSystem.ExamStepUpdated -= OnExamStepUpdated;
        }

        private void OnExamStarted(StartExam component)
        {
            Model = dataSource.Load<ExamStatisticsModel>() ?? new ExamStatisticsModel();
            Model.StartExam(component.Module);
            
            if (source.Debug)
                Debug.Log($"{nameof(ExamStarted)}: identifier:{component.Module.ModuleIdentifier}");
            dataSource.Save(Model);
            ExamStarted?.Invoke();
        }
        private void OnExamStopped(bool passed)
        {
            Model.StopExam(passed ? ExamStatus.Completed : ExamStatus.Failed);
            
            if (source.Debug)
                Debug.Log($"{nameof(ExamStopped)}: passed:{passed}");
            dataSource.Save(Model);
            ExamStopped?.Invoke();
        }

        private void OnExamStepStarted(ExamStep step)
        {
            Model.StartStep(step);
            
            if (source.Debug)
                Debug.Log($"{nameof(ExamStepStarted)}: identifier:{step.StepIdentifier}");
            dataSource.Save(Model);
            ExamStepStarted?.Invoke();
        }
        private void OnExamStepStopped(ExamStep step, bool isPassed)
        {
            Model.StopStep(step, isPassed);
            
            if (source.Debug)
                Debug.Log($"{nameof(ExamStepStopped)}: passed:{isPassed}, identifier:{step.StepIdentifier}");
            dataSource.Save(Model);
            ExamStepStopped?.Invoke();
        }
        private void OnExamStepUpdated(ExamStep step, bool isPassed, bool overrideTime)
        {
            Model.UpdateStep(step, isPassed, overrideTime);
            
            if (source.Debug)
                Debug.Log($"{nameof(ExamStepUpdated)}: passed:{isPassed}, " +
                          $"overrideTime:{overrideTime} identifier:{step.StepIdentifier}");
            dataSource.Save(Model);
            ExamStepUpdated?.Invoke();
        }

        public IState GetState() => Model;

        public void SetState(IState state) => Model = (ExamStatisticsModel)state;
    }
}