using System;
using System.Collections.Generic;

namespace VRF.Scenario.Models
{
    public enum ExamStatus
    {
        None = 0,       // Экзамен не был начат и не имеет статуса
        InProgress = 1, // В процессе прохождения
        Completed = 2,  // Пройден
        Failed = 3,     // Провален, причины могут быть разные
        Cancelled = 4,  // Отменён
    }
    public enum StepStatus
    {
        None = 0,       // Шаг не был начат
        InProgress = 1, // Шаг в процессе выполнения
        Completed = 2,  // Шаг был выполнен
        Failed = 3,     // Шаг был провален
    }
        
    public struct ScenarioIdentity
    {
        public string ModuleName;
        public string ModuleIdentifier;
    }
    public struct ClientIdentity
    {
        public string FirstName;  // Имя
        public string LastName;   // Фамилия
        public string Patronymic; // Отчество
        public Dictionary<string, string> AdditionalData;
    }
    public struct Exam
    {
        public ExamStatus Status;
        public DateTime StartTime;
        public DateTime StopTime;
        
        public int StepsCompleted;
        public int StepCount;

        public TimeSpan TimeSpan => StopTime - StartTime;
        public float Progress => StepCount == 0 ? 0 : StepsCompleted / (float)StepCount;
        public float Percentage => Progress * 100f;
    }
    public struct Step
    {
        public StepStatus Status;
        public string StepText;
        public string StepIdentifier;
        public DateTime StartTime;
        public DateTime StopTime;
        public Dictionary<string, string> AdditionalData;
        
        public TimeSpan TimeSpan => StopTime - StartTime;
    }
    public struct Event
    {
        public string EventName;
        public string EventIdentifier;
        public DateTime StartTime;
        public DateTime StopTime;
        public Dictionary<string, string> AdditionalData;
        
        public TimeSpan TimeSpan => StopTime - StartTime;
    }
}