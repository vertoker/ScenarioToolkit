using System;
using System.Collections.Generic;
using Scenario.Core.Scriptables;
using Scenario.Core.Systems.States;

namespace VRF.Scenario.Models
{
    [Serializable]
    public class ExamStatisticsModel : IState
    {
        public ScenarioIdentity ScenarioIdentity;
        public ClientIdentity ClientIdentity;
        public Exam Exam;
        
        public Dictionary<string, Step> Steps = new();
        public Dictionary<string, Event> Events = new();
        public Dictionary<string, List<string>> AdditionalData = new();
        
        public readonly int Version = 1;

        #region Exam
        public void StartExam(ScenarioModule module)
        {
            ScenarioIdentity = new ScenarioIdentity
            {
                ModuleName = module.ModuleName,
                ModuleIdentifier = module.ModuleIdentifier,
            };
            //ClientIdentity = new ClientIdentity();
            Exam = new Exam
            {
                Status = ExamStatus.InProgress,
                StartTime = DateTime.Now,
                StopTime = DateTime.Now,
                StepsCompleted = 0,
                StepCount = module.ExamSteps.Length
            };
            
            Steps.Clear();
            Events.Clear();
            AdditionalData.Clear();
        }
        public void StopExam(ExamStatus status)
        {
            Exam.Status = status;
            Exam.StopTime = DateTime.Now;
        }
        #endregion

        #region Step
        public void StartStep(ExamStep step)
        {
            if (Steps.ContainsKey(step.StepIdentifier)) return;
            
            var stepData = new Step
            {
                Status = StepStatus.InProgress,
                StartTime = DateTime.Now,
                StopTime = DateTime.Now,
                StepIdentifier = step.StepIdentifier,
                StepText = step.StepText,
                AdditionalData = step.AdditionalData,
            };
            Steps.Add(step.StepIdentifier, stepData);
        }
        public void StopStep(ExamStep step, bool isPassed)
        {
            if (!Steps.TryGetValue(step.StepIdentifier, out var stepData)) return;
            
            stepData.Status = isPassed ? StepStatus.Completed : StepStatus.Failed;
            stepData.StopTime = DateTime.Now;
            Steps[step.StepIdentifier] = stepData;
            Exam.StepCount++;
        }
        public void ClearStep(ExamStep step)
        {
            if (Steps.Remove(step.StepIdentifier))
                Exam.StepCount--;
        }
        
        public void UpdateStep(ExamStep step, bool isPassed, bool overrideTime)
        {
            if (!Steps.TryGetValue(step.StepIdentifier, out var stepData)) return;
            
            stepData.Status = isPassed ? StepStatus.Completed : StepStatus.Failed;
            if (overrideTime) stepData.StopTime = DateTime.Now;
            
            Steps[step.StepIdentifier] = stepData;
        }
        #endregion

        #region Event
        public void StartEvent(string eventIdentifier, string eventName)
        {
            if (!Events.ContainsKey(eventIdentifier)) return;
            
            var stepData = new Event
            {
                EventName = eventName,
                EventIdentifier = eventIdentifier,
                StartTime = DateTime.Now,
                StopTime = DateTime.Now,
            };
            Events.Add(eventIdentifier, stepData);
        }
        public void StopEvent(string eventIdentifier)
        {
            if (!Events.TryGetValue(eventIdentifier, out var eventData)) return;
            
            eventData.StopTime = DateTime.Now;
            //eventData.AdditionalData.AddRange(additionalData);
            Events[eventIdentifier] = eventData;
        }
        public void ClearEvent(string eventIdentifier)
        {
            Events.Remove(eventIdentifier);
        }
        #endregion
        
        public string TotalTimeAsString() => (Exam.StopTime - Exam.StartTime).ToString(@"hh\:mm\:ss");

        #region Variants

        public static readonly ExamStatisticsModel TestTemplate1 = new() {
            ScenarioIdentity = new ScenarioIdentity
            {
                ModuleIdentifier = "test-scenario-1",
                ModuleName = "Тестовое имя 12321",
            },
            ClientIdentity = new ClientIdentity
            {
                FirstName = "Константин",
                LastName = "Чураков",
                Patronymic = "Эдуардович",
                AdditionalData = new Dictionary<string, string>
                {
                    { "role1", "programmer" },
                    { "role2", "Программист" },
                },
            },
            Exam = new Exam
            {
                StartTime = DateTime.Now,
                StopTime = DateTime.Now + TimeSpan.FromSeconds(61.5d),
                Status = ExamStatus.Completed,
                StepCount = 3,
                StepsCompleted = 2,
            },
            Steps = new Dictionary<string, Step>
            {
                {
                    "step-1",
                    new Step
                    {
                        Status = StepStatus.Completed,
                        StepText = "Шаг 1",
                        StepIdentifier = "step-1",
                        StartTime = DateTime.Now,
                        StopTime = DateTime.Now + TimeSpan.FromSeconds(10),
                        AdditionalData = new Dictionary<string, string>
                        {
                            { "data1", "qwerty" },
                            { "data2", "asdf" },
                        },
                    }
                },
                {
                    "step-2",
                    new Step
                    {
                        Status = StepStatus.InProgress,
                        StepText = "Шаг 2",
                        StepIdentifier = "step-2",
                        StartTime = DateTime.Now,
                        StopTime = DateTime.Now + TimeSpan.FromSeconds(20),
                        AdditionalData = new Dictionary<string, string>
                        {
                            { "data3", "qwerty2" },
                            { "data4", "asdf2" },
                        },
                    }
                },
                {
                    "step-3",
                    new Step
                    {
                        Status = StepStatus.Failed,
                        StepText = "Шаг 3",
                        StepIdentifier = "step-3",
                        StartTime = DateTime.Now,
                        StopTime = DateTime.Now + TimeSpan.FromSeconds(30),
                        AdditionalData = new Dictionary<string, string>
                        {
                            { "data5", "qwerty3" },
                            { "data6", "asdf3" },
                        },
                    }
                },
            }
        };

        #endregion

        public void Clear()
        {
            // Тут не надо
        }
    }
}