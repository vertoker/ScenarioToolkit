using System;
using System.Collections.Generic;
using System.Linq;
using Scenario.Core.DataSource;
using Scenario.Core.Scriptables;
using SimpleUI.Core;
using SimpleUI.Installers.ModelSettings;
using UnityEngine;

namespace VRF.Scenario.UI.ScenarioMenu
{
    public class ScreenSettingsInstaller : BaseScreenSettingsInstaller<ScreenSettingsModel> { }
    
    [Serializable]
    public class ScreenSettingsModel : BaseScreenSettingsModel
    {
        [SerializeField] private ScenarioModules modules;
        [Header("Menu")]
        [SerializeField] private ButtonsConfig buttonsConfig = ButtonsConfig.EnumFilter;
        [SerializeField] private string startName = "Начать";
        [SerializeField] private string modeStudyName = "Обучение";
        [SerializeField] private string modeExamName = "Экзамен";
        [SerializeField] private string aboutName = "О разработчиках";
        
        public enum ButtonsConfig
        {
            EnumFilter = 0,
            ButtonsFilter = 1,
        }
        
        public enum ScenarioFilterMode
        {
            Valid = 0,
            Mode = 1,
            All = 2,
        }
        
        public ScenarioModules Modules => modules;

        public ButtonsConfig ButtonsConfiguration => buttonsConfig;
        
        public string StartName => startName;
        public string ModeStudyName => modeStudyName;
        public string ModeExamName => modeExamName;
        public string AboutName => aboutName;

        public IEnumerable<ScenarioModule> GetConfigs(ScenarioMode mode)
        {
            return mode switch
            {
                ScenarioMode.Study => modules.FilteredModules(ScenarioMode.Study),
                ScenarioMode.Exam => modules.FilteredModules(ScenarioMode.Exam),
                _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
            };
        }
    }
}