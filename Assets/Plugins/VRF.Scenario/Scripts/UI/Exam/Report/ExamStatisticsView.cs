using System;
using System.Collections.Generic;
using SimpleUI.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRF.DataSources.LocalCache;
using VRF.Scenario.Models;
using VRF.Scenario.UI.Exam.Report.Data;
using Zenject;
using Object = UnityEngine.Object;

namespace VRF.Scenario.UI.Exam.Report
{
    public class ExamStatisticsView : UIView
    {
        [Header("Core")]
        [SerializeField] private RectTransform viewport;
        [SerializeField] private GameObject examPanel;
        [SerializeField] private GameObject stepsPanel;
        [SerializeField] private Scrollbar examScrollbar;
        [SerializeField] private Scrollbar stepsScrollbar;
        [SerializeField] private TMP_Text backTitle;
        
        [Header("Exam")]
        [SerializeField] private TMP_Text examTitle;
        [SerializeField] private LabelValueBind examName;
        [SerializeField] private LabelValueBind examStatus;
        [SerializeField] private LabelValueBind examTime;
        [SerializeField] private LabelValueBind examPercent;
        [Space]
        [SerializeField] private LabelValueBind firstName;
        [SerializeField] private LabelValueBind lastName;
        [SerializeField] private LabelValueBind patronymic;
        
        [Header("Steps")]
        [SerializeField] private TMP_Text stepsTitle;
        [SerializeField] private StepBind stepTemplate;
        [SerializeField] private Transform stepsContent;

        public TMP_Text ExamTitle => examTitle;
        public LabelValueBind ExamName => examName;
        public LabelValueBind ExamStatus => examStatus;
        public LabelValueBind ExamTime => examTime;
        public LabelValueBind ExamPercent => examPercent;
        
        public LabelValueBind FirstName => firstName;
        public LabelValueBind LastName => lastName;
        public LabelValueBind Patronymic => patronymic;

        public TMP_Text StepsTitle => stepsTitle;
        public StepBind StepTemplate => stepTemplate;
        public Transform StepsContent => stepsContent;

        public RectTransform Viewport => viewport;
        public GameObject ExamPanel => examPanel;
        public GameObject StepsPanel => stepsPanel;
        public Scrollbar ExamScrollbar => examScrollbar;
        public Scrollbar StepsScrollbar => stepsScrollbar;
        public TMP_Text BackTitle => backTitle;

        public override Type GetControllerType() => typeof(ExamStatisticsController);
    }
    
    public class ExamStatisticsController : UIController<ExamStatisticsView>, IInitializable, IDisposable
    {
        private readonly LocalCacheDataSource dataSource;
        private readonly ScreenSettingsModel settingsModel;

        private ExamStatisticsModel examStatisticsModel;
        private Dictionary<string, StepBind> steps;

        public ExamStatisticsController(LocalCacheDataSource dataSource,
            [InjectOptional] ScreenSettingsModel settingsModel,
            ExamStatisticsView view) : base(view)
        {
            this.dataSource = dataSource;
            this.settingsModel = settingsModel;
        }

        public void Initialize()
        {
            View.Screen.Opened += OnOpen;
            
            // флаг для запуска экзамена
            if (settingsModel.UseTestModel || dataSource.Remove<ExamStatisticsModelFlag>())
                View.Manager.Open<ExamStatisticsScreen>();
                //View.Screen.Open(false);
        }

        public void Dispose()
        {
            View.Screen.Opened -= OnOpen;
        }

        private void OnOpen(ScreenBase screen)
        {
            examStatisticsModel = settingsModel.UseTestModel 
                ? ExamStatisticsModel.TestTemplate1 
                : dataSource.Load<ExamStatisticsModel>();

            if (examStatisticsModel == null)
            {
                Debug.LogError($"Can't find {nameof(ExamStatisticsModel)} in local cache, create empty model");
                examStatisticsModel = new ExamStatisticsModel();
            }

            if (steps == null)
                CreateStepsElements();
            SetSettingsModel();
            LoadDataFromModel();
        }

        private void CreateStepsElements()
        {
            steps = new Dictionary<string, StepBind>();
            foreach (var step in examStatisticsModel.Steps)
            {
                var stepBind = Object.Instantiate(View.StepTemplate, View.StepsContent);
                //stepBind.gameObject.SetActive(false);
                steps.Add(step.Key, stepBind);
            }
        }

        private void SetSettingsModel()
        {
            View.ExamPanel.SetActive(settingsModel.ShowExamPanel);
            View.StepsPanel.SetActive(settingsModel.ShowStepsPanel);
            View.ExamTitle.gameObject.SetActive(settingsModel.ShowAllPanel);
            View.StepsTitle.gameObject.SetActive(settingsModel.ShowAllPanel);
            
            View.ExamTitle.text = settingsModel.ExamTitle;
            View.StepsTitle.text = settingsModel.StepsTitle;
            View.BackTitle.text = settingsModel.BackTitle;

            if (settingsModel.ShowAllPanel)
            {
                View.Viewport.anchorMin = new Vector2(0, 0);
                View.Viewport.anchorMax = new Vector2(1, 1);
            }
            else
            {
                View.Viewport.anchorMin = new Vector2(0.5f, 0);
                View.Viewport.anchorMax = new Vector2(0.5f, 1);
                View.Viewport.sizeDelta = new Vector2(settingsModel.DefaultWidthPanel, View.Viewport.sizeDelta.y);
            }
            
            View.ExamName.SetActive(settingsModel.ShowExamName);
            View.ExamStatus.SetActive(settingsModel.ShowExamStatus);
            View.ExamTime.SetActive(settingsModel.ShowExamTime);
            View.ExamPercent.SetActive(settingsModel.ShowExamPercent);

            View.FirstName.SetActive(settingsModel.ShowFirstName);
            View.LastName.SetActive(settingsModel.ShowLastName);
            View.Patronymic.SetActive(settingsModel.ShowPatronymic);

            foreach (var step in steps.Values)
                step.SetActive(settingsModel.ShowStepName, settingsModel.ShowStepStatus, settingsModel.ShowStepTime);
        }

        private void LoadDataFromModel()
        {
            View.ExamName.SetValue(examStatisticsModel.ScenarioIdentity.ModuleName);
            View.ExamStatus.SetStatus(examStatisticsModel.Exam.Status);
            View.ExamTime.SetTime(examStatisticsModel.Exam.TimeSpan);
            View.ExamPercent.SetValue($"{examStatisticsModel.Exam.Percentage}%");

            View.FirstName.SetValue(examStatisticsModel.ClientIdentity.FirstName);
            View.LastName.SetValue(examStatisticsModel.ClientIdentity.LastName);
            View.Patronymic.SetValue(examStatisticsModel.ClientIdentity.Patronymic);

            foreach (var stepDataBind in examStatisticsModel.Steps)
            {
                var step = steps[stepDataBind.Key];
                step.Set(stepDataBind.Value);
                step.gameObject.SetActive(true);
            }

            View.ExamScrollbar.value = 0;
            View.StepsScrollbar.value = 0;
        }
    }
}