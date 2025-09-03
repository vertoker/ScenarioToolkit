using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Scenario.Core.DataSource;
using Scenario.Core.Scriptables;
using SimpleUI.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRF.DataSources.LocalCache;
using VRF.Identities;
using VRF.Identities.Core;
using VRF.Scenes.Project;
using Zenject;

namespace VRF.Scenario.UI.ScenarioMenu.Selector
{
    public class ScenarioViewerView : UIView
    {
        [SerializeField] private GameObject parent;
        [SerializeField] private TMP_Text title;
        [SerializeField] private TMP_Text description;
        [SerializeField] private TMP_Dropdown dropdown;
        [SerializeField] private Button button;

        public GameObject Parent => parent;
        public TMP_Text Title => title;
        public TMP_Text Description => description;
        public TMP_Dropdown Dropdown => dropdown;
        public Button Button => button;

        public override Type GetControllerType() => typeof(ScenarioViewerController);
    }

    public class ScenarioViewerController : UIController<ScenarioViewerView>, IInitializable, IDisposable
    {
        private readonly LocalCacheDataSource dataSource;
        private readonly ScenesService scenesService;
        private readonly ScreenSettingsModel settingsModel;
        [CanBeNull] private readonly IdentityService identityService;

        private readonly TMP_Dropdown.OptionData study;
        private readonly TMP_Dropdown.OptionData exam;
        private readonly List<TMP_Dropdown.OptionData> dropdownCache;

        // полная хуйня, ui зашёл куда-то не туда
        public ScenarioModule CurrentModule { get; private set; }
        public bool HasCurrent => CurrentModule;
        
        public ScenarioViewerController(LocalCacheDataSource dataSource, ScenesService scenesService,
            [InjectOptional] ScreenSettingsModel settingsModel, 
            [InjectOptional] IdentityService identityService, 
            ScenarioViewerView view) : base(view)
        {
            this.dataSource = dataSource;
            this.scenesService = scenesService;
            this.settingsModel = settingsModel;
            this.identityService = identityService;

            if (settingsModel != null)
            {
                study = new TMP_Dropdown.OptionData(settingsModel.ModeStudyName);
                exam = new TMP_Dropdown.OptionData(settingsModel.ModeExamName);
            }
            
            dropdownCache = new List<TMP_Dropdown.OptionData>(Enum.GetValues(typeof(ScenarioMode)).Length);
            View.Dropdown.options = dropdownCache;
            View.Dropdown.value = 0;
            
            Clear();
        }

        public void Initialize()
        {
            View.Screen.Opened += OnOpened;
            View.Screen.Closed += OnClosed;
            View.Button.onClick.AddListener(OnClick);
        }
        public void Dispose()
        {
            View.Screen.Opened -= OnOpened;
            View.Screen.Closed -= OnClosed;
            View.Button.onClick.RemoveListener(OnClick);
        }

        private void OnOpened(ScreenBase screen)
        {
            //ClearScenarioMode();
            
            if (settingsModel.ButtonsConfiguration == ScreenSettingsModel.ButtonsConfig.ButtonsFilter)
                View.Dropdown.gameObject.SetActive(false);
        }
        private void OnClosed(ScreenBase screen)
        {
            Clear();
        }
        
        private void OnClick()
        {
            if (CurrentModule)
            {
                var model = CurrentModule.GetModel(identityService?.SelfIdentity);

                dataSource.Save(model);
                scenesService.LoadScene(CurrentModule.ScenePath);
            }
        }

        public void Show(ScenarioModule module)
        {
            View.Title.text = module.ModuleName;
            View.Description.text = module.ModuleDescription;
            
            dropdownCache.Clear();
            if (module.Mode == ScenarioMode.Study) dropdownCache.Add(study);
            if (module.Mode == ScenarioMode.Exam) dropdownCache.Add(exam);
            View.Dropdown.value = 0;
            
            if (settingsModel.ButtonsConfiguration == ScreenSettingsModel.ButtonsConfig.ButtonsFilter)
                View.Dropdown.gameObject.SetActive(false);
            else View.Dropdown.gameObject.SetActive(dropdownCache.Count > 0);

            CurrentModule = module;
            View.Button.gameObject.SetActive(HasCurrent);
            
            View.Parent.SetActive(true);
        }
        public void Clear()
        {
            View.Parent.SetActive(false);
            
            View.Title.text = string.Empty;
            View.Description.text = string.Empty;
            
            CurrentModule = null;
            View.Button.gameObject.SetActive(false);
            
            dropdownCache.Clear();
            View.Dropdown.value = 0;
            View.Dropdown.gameObject.SetActive(false);
        }
    }
}