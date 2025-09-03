using System;
using SimpleUI.Core;
using TMPro;
using UnityEngine;
using Zenject;

namespace VRF.Scenario.UI.ScenarioMenu
{
    public class ScenarioMenuView : UIView
    {
        [Header("Enum Buttons")]
        [SerializeField] private GameObject enumButtons;
        [SerializeField] private TMP_Text startBtnText;
        [SerializeField] private TMP_Text aboutBtnText;

        [Header("Buttons Buttons")]
        [SerializeField] private GameObject buttonsButtons;
        [SerializeField] private TMP_Text modeStudyBtnText;
        [SerializeField] private TMP_Text modeExamBtnText;
        [SerializeField] private TMP_Text aboutBtnText2;
        
        public GameObject EnumButtons => enumButtons;
        public GameObject ButtonsButtons => buttonsButtons;
        
        public TMP_Text StartBtnText => startBtnText;
        public TMP_Text ModeStudyBtnText => modeStudyBtnText;
        public TMP_Text ModeExamBtnText => modeExamBtnText;
        public TMP_Text AboutBtnText => aboutBtnText;
        public TMP_Text AboutBtnText2 => aboutBtnText2;
        
        public override Type GetControllerType() => typeof(ScenarioMenuController);
    }
    
    public class ScenarioMenuController : UIController<ScenarioMenuView>
    {
        public ScenarioMenuController([InjectOptional] ScreenSettingsModel settingsModel, ScenarioMenuView view) : base(view)
        {
            if (settingsModel == null) return;
            
            View.EnumButtons?.SetActive(false);
            View.ButtonsButtons?.SetActive(false);
            
            switch (settingsModel.ButtonsConfiguration)
            {
                case ScreenSettingsModel.ButtonsConfig.EnumFilter:
                    View.StartBtnText?.SetText(settingsModel.StartName);
                    View.AboutBtnText?.SetText(settingsModel.AboutName);
                    View.EnumButtons?.SetActive(true);
                    break;
                case ScreenSettingsModel.ButtonsConfig.ButtonsFilter:
                    View.ModeStudyBtnText?.SetText(settingsModel.ModeStudyName);
                    View.ModeExamBtnText?.SetText(settingsModel.ModeExamName);
                    View.AboutBtnText2?.SetText(settingsModel.AboutName);
                    View.ButtonsButtons?.SetActive(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}