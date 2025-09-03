using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using SimpleUI.Installers.ModelSettings;
using UnityEngine;
using VRF.Scenario.UI.Exam.Report.Data;

namespace VRF.Scenario.UI.Exam.Report
{
    public class ScreenSettingsInstaller : BaseScreenSettingsInstaller<ScreenSettingsModel> { }
    
    [Serializable]
    public class ScreenSettingsModel : BaseScreenSettingsModel
    {
        [SerializeField] private bool useTestModel = false;
        [SerializeField] private float defaultWidthPanel = 900f;
        [SerializeField] private string backTitle = "Продолжить";
        [Header("Exam")]
        [SerializeField] private bool showExamPanel = true;
        [SerializeField] private string examTitle = "Экзамен";
        [Space]
        [SerializeField] private bool showExamName = true;
        [SerializeField] private bool showExamStatus = true;
        [SerializeField] private bool showExamTime = true;
        [SerializeField] private bool showExamPercent = true;
        [Space]
        [SerializeField] private bool showFirstName = true;
        [SerializeField] private bool showLastName = true;
        [SerializeField] private bool showPatronymic = true;
        
        [Header("Steps")]
        [SerializeField] private bool showStepsPanel = true;
        [SerializeField] private string stepsTitle = "Шаги";
        [Space]
        [SerializeField] private bool showStepName = true;
        [SerializeField] private bool showStepStatus = true;
        [SerializeField] private bool showStepTime = true;
        [Space]
        [SerializeField] private SerializedDictionary<string, StepBind.Input> additionalStepData = new();

        public bool ShowExamName => showExamName;
        public bool ShowExamStatus => showExamStatus;
        public bool ShowExamTime => showExamTime;
        public bool ShowExamPercent => showExamPercent;

        public bool ShowFirstName => showFirstName;
        public bool ShowLastName => showLastName;
        public bool ShowPatronymic => showPatronymic;

        public bool ShowStepName => showStepName;
        public bool ShowStepStatus => showStepStatus;
        public bool ShowStepTime => showStepTime;

        public IReadOnlyDictionary<string, StepBind.Input> AdditionalStepData => additionalStepData;

        public float DefaultWidthPanel => defaultWidthPanel;
        public bool ShowExamPanel => showExamPanel;
        public bool ShowStepsPanel => showStepsPanel;
        public bool ShowAllPanel => showExamPanel && showStepsPanel;

        public string BackTitle => backTitle;
        public string ExamTitle => examTitle;
        public string StepsTitle => stepsTitle;

        public bool UseTestModel => useTestModel;
    }
}