using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using NaughtyAttributes;
using Scenario.Core.DataSource;
using SimpleUI.Installers.ModelSettings;
using UnityEngine;

namespace VRF.Scenario.UI.Exam.InputClient
{
    public class ScreenSettingsInstaller : BaseScreenSettingsInstaller<ScreenSettingsModel> { }

    [Serializable]
    public class ScreenSettingsModel : BaseScreenSettingsModel
    {
        [Header("Core")]
        [SerializeField] private bool showFirstName = true;
        //[EnableIf(nameof(showFirstName))]
        //[SerializeField] private bool requiredFirstName = true;
        
        [SerializeField] private bool showLastName = true;
        //[EnableIf(nameof(showLastName))]
        //[SerializeField] private bool requiredLastName = true;
        
        [SerializeField] private bool showPatronymic = true;
        //[EnableIf(nameof(showPatronymic))]
        //[SerializeField] private bool requiredPatronymic = false;
        
        [Header("ScenarioMenu")]
        [SerializeField] private bool useScenarioMenu = true;
        [EnableIf(nameof(useScenarioMenu))]
        [SerializeField] private ScenarioMenu.ScreenSettingsModel.ScenarioFilterMode filterMode = 
            ScenarioMenu.ScreenSettingsModel.ScenarioFilterMode.Mode;
        [EnableIf(nameof(useScenarioMenu))]
        [SerializeField] private ScenarioMode scenarioMode = ScenarioMode.Exam;

        [Header("Additional")]
        [SerializeField] private SerializedDictionary<string, InputClientController.FieldBind> showAdditionalData = new();

        public bool ShowFirstName => showFirstName;
        //public bool RequiredFirstName => requiredFirstName;
        public bool ShowLastName => showLastName;
        //public bool RequiredLastName => requiredLastName;
        public bool ShowPatronymic => showPatronymic;
        //public bool RequiredPatronymic => requiredPatronymic;
        
        public bool UseScenarioMenu => useScenarioMenu;
        public ScenarioMenu.ScreenSettingsModel.ScenarioFilterMode ScenarioFilterMode => filterMode;
        public ScenarioMode ScenarioMode => scenarioMode;

        public IReadOnlyDictionary<string, InputClientController.FieldBind> ShowAdditionalData => showAdditionalData;
    }
}