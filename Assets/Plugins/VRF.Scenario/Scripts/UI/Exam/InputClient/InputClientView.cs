using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Newtonsoft.Json;
using SimpleUI.Core;
using SimpleUI.Templates;
using TMPro;
using UnityEngine;
using VRF.DataSources.LocalCache;
using VRF.Scenario.Models;
using VRF.Scenario.UI.ScenarioMenu;
using VRF.UI.Menu;
using Zenject;
using Object = UnityEngine.Object;

namespace VRF.Scenario.UI.Exam.InputClient
{
    public class InputClientView : UIView
    {
        // Если модель найдётся, то данные из неё будут загружены в поля
        [SerializeField] private bool restoreDataIfFound = false;

        [Header("Core Data")]
        [SerializeField] private TMP_InputField firstName;
        [SerializeField] private TMP_InputField lastName;
        [SerializeField] private TMP_InputField patronymic;
        
        [Header("Buttons Data")]
        [SerializeField] private SwitchScreenButtonView backBtn;
        [SerializeField] private SwitchScreenButtonView continueBtn;

        [Header("Additional Data")]
        [SerializeField] private TMP_InputField template;
        [SerializeField] private Transform additionalDataContent;

        public bool RestoreDataIfFound => restoreDataIfFound;

        public TMP_InputField FirstName => firstName;
        public TMP_InputField LastName => lastName;
        public TMP_InputField Patronymic => patronymic;

        public TMP_InputField Template => template;
        public Transform AdditionalDataContent => additionalDataContent;

        public SwitchScreenButtonView BackBtn => backBtn;
        public SwitchScreenButtonView ContinueBtn => continueBtn;

        public override Type GetControllerType() => typeof(InputClientController);
    }

    public class InputClientController : UIController<InputClientView>, IInitializable, IDisposable
    {
        private readonly LocalCacheDataSource dataSource;
        [CanBeNull] private readonly ScreenSettingsModel settingsModel;
        
        private readonly Dictionary<string, TMP_InputField> additionalElements;
        private ExamStatisticsModel model;
        
        [CanBeNull] private SwitchScreenButtonView switchView;

        [Serializable]
        public class FieldBind
        {
            public bool active = true;
            public string placeholderText = string.Empty;
            public TMP_InputField.ContentType сontentType = TMP_InputField.ContentType.Standard;
        }

        public InputClientController(LocalCacheDataSource dataSource,
            [InjectOptional] ScreenSettingsModel settingsModel,
            InputClientView view) : base(view)
        {
            this.dataSource = dataSource;
            this.settingsModel = settingsModel;
            if (this.settingsModel != null)
                additionalElements = CreateAdditionalElements(this.settingsModel.ShowAdditionalData);
        }

        public void Initialize()
        {
            View.Screen.Opened += OnOpen;
            View.Screen.Closed += OnClose;
            View.ContinueBtn.Button.onClick.AddListener(SaveModel);

            InitializeViews();
            
            if (switchView)
            {
                (switchView.ScreenToSwitch, View.ContinueBtn.ScreenToSwitch) 
                    = (View.ContinueBtn.ScreenToSwitch, switchView.ScreenToSwitch);
                //View.BackBtn.ScreenToSwitch = screen;
            }
        }
        private void InitializeViews()
        {
            if (settingsModel == null) return;
            if (!settingsModel.UseScenarioMenu) return;

            var screen = View.Manager.FindInList<MenuScreen>();
            if (!screen) return;

            var filterViews = screen.GetAll<ScenarioListFilterView>();
            var filterView = filterViews.FirstOrDefault(fv => fv.Compare(settingsModel.ScenarioFilterMode, settingsModel.ScenarioMode));
            if (!filterView)
            {
                Debug.LogError($"Empty filter view", View);
                return;
            }
            
            switchView = filterView.GetComponent<SwitchScreenButtonView>();
        }
        
        public void Dispose()
        {
            View.Screen.Opened -= OnOpen;
            View.Screen.Closed -= OnClose;
            View.ContinueBtn.Button.onClick.RemoveListener(SaveModel);
        }

        private void OnOpen(ScreenBase screen)
        {
            model = dataSource.Load<ExamStatisticsModel>() ?? new ExamStatisticsModel();

            SetActivityElements();

            if (View.RestoreDataIfFound)
                LoadDataFromModel();
            else ResetData();
        }
        private void OnClose(ScreenBase screen)
        {
            //SaveModel();
        }

        public void SaveModel()
        {
            SaveDataToModel();
            Debug.Log($"Saved {nameof(ClientIdentity)}: {JsonConvert.SerializeObject(model.ClientIdentity)}");
            dataSource.Save(model);
        }

        private Dictionary<string, TMP_InputField> CreateAdditionalElements(
            IReadOnlyDictionary<string, FieldBind> additionalData)
        {
            var newAdditionalElements = new Dictionary<string, TMP_InputField>();
            foreach (var additional in additionalData)
            {
                var field = Object.Instantiate(View.Template, View.AdditionalDataContent);
                var text = field.placeholder.GetComponent<TMP_Text>();

                field.name = additional.Value.placeholderText;
                field.contentType = additional.Value.сontentType;
                text.text = additional.Value.placeholderText;

                newAdditionalElements.Add(additional.Key, field);
            }

            return newAdditionalElements;
        }
        
        private void SetActivityElements()
        {
            if (settingsModel == null) return;
            
            if (View.FirstName) View.FirstName.gameObject.SetActive(settingsModel.ShowFirstName);
            if (View.LastName) View.LastName.gameObject.SetActive(settingsModel.ShowLastName);
            if (View.Patronymic) View.Patronymic.gameObject.SetActive(settingsModel.ShowPatronymic);

            if (additionalElements == null) return;

            foreach (var entry in settingsModel.ShowAdditionalData)
            {
                if (additionalElements.TryGetValue(entry.Key, out var field) && field)
                    field.gameObject.SetActive(entry.Value.active);
            }
        }

        private void LoadDataFromModel()
        {
            if (View.FirstName) View.FirstName.text = model.ClientIdentity.FirstName;
            if (View.LastName) View.LastName.text = model.ClientIdentity.LastName;
            if (View.Patronymic) View.Patronymic.text = model.ClientIdentity.Patronymic;

            if (additionalElements == null) return;

            foreach (var entry in additionalElements.Where(entry => entry.Value))
                if (model.ClientIdentity.AdditionalData.TryGetValue(entry.Key, out var text))
                    entry.Value.text = text;
        }
        private void ResetData()
        {
            if (View.FirstName) View.FirstName.text = string.Empty;
            if (View.LastName) View.LastName.text = string.Empty;
            if (View.Patronymic) View.Patronymic.text = string.Empty;

            if (additionalElements == null) return;

            foreach (var entry in additionalElements.Where(entry => entry.Value))
                entry.Value.text = string.Empty;
        }

        private void SaveDataToModel()
        {
            if (View.FirstName) model.ClientIdentity.FirstName = View.FirstName.text;
            if (View.LastName) model.ClientIdentity.LastName = View.LastName.text;
            if (View.Patronymic) model.ClientIdentity.Patronymic = View.Patronymic.text;

            if (additionalElements == null) return;

            foreach (var entry in additionalElements.Where(entry => entry.Value))
                model.ClientIdentity.AdditionalData[entry.Key] = entry.Value.text;
        }
    }
}