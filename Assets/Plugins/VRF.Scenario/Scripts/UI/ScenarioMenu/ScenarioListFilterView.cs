using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using NaughtyAttributes;
using Scenario.Core.DataSource;
using Scenario.Core.Scriptables;
using SimpleUI.Templates.Base;
using UnityEngine;
using UnityEngine.Events;
using VRF.Scenario.UI.ScenarioMenu.Selector;
using Zenject;

namespace VRF.Scenario.UI.ScenarioMenu
{
    public class ScenarioListFilterView : BaseButtonView
    {
        [SerializeField] private ScreenSettingsModel.ButtonsConfig selfConfiguration =
            ScreenSettingsModel.ButtonsConfig.EnumFilter;
        [SerializeField] private ScreenSettingsModel.ScenarioFilterMode filterMode = 
            ScreenSettingsModel.ScenarioFilterMode.Valid;
        [ShowIf(nameof(FilterModeIsMode))]
        [SerializeField] private ScenarioMode scenarioMode = ScenarioMode.Study;

        private bool FilterModeIsMode => filterMode == ScreenSettingsModel.ScenarioFilterMode.Mode;
        
        public ScreenSettingsModel.ButtonsConfig SelfConfiguration => selfConfiguration;
        public ScreenSettingsModel.ScenarioFilterMode FilterMode => filterMode;
        public ScenarioMode ScenarioMode => scenarioMode;

        public bool Compare(ScreenSettingsModel.ScenarioFilterMode fm, ScenarioMode sm)
            => filterMode == fm && scenarioMode == sm;
        
        public override Type GetControllerType() => typeof(ScenarioListFilterController);
    }
    public class ScenarioListFilterController : BaseButtonController<ScenarioListFilterView>
    {
        [CanBeNull] private readonly ScreenSettingsModel settingsModel;
        private ScenarioListController listController;
        private ScenarioViewerController viewerController;

        public ScenarioListFilterController([InjectOptional] ScreenSettingsModel settingsModel,
            ScenarioListFilterView view) : base(view)
        {
            this.settingsModel = settingsModel;
        }

        public override void Initialize()
        {
            if (settingsModel == null) return;
            // Оптимизация, чтобы не инициализировать кнопки если их нет
            //if (settingsModel.ListConfiguration != View.SelfConfiguration) return;
            
            var selectorScreen = View.Manager.FindInList<ScenarioSelectorScreen>();
            selectorScreen.TryGetController<ScenarioListView, ScenarioListController>(false, out listController);
            selectorScreen.TryGetController<ScenarioViewerView, ScenarioViewerController>(false, out viewerController);
            
            base.Initialize();
        }

        protected override UnityAction GetAction() => SetFilter;
        
        private void SetFilter()
        {
            if (settingsModel == null) return;

            var modules = GetModules();
            listController.Setup(modules);
        }

        private IEnumerable<ScenarioModule> GetModules()
        {
            if (settingsModel != null)
                return View.FilterMode switch
                {
                    ScreenSettingsModel.ScenarioFilterMode.Valid
                        => settingsModel.Modules.ValidModules,
                    ScreenSettingsModel.ScenarioFilterMode.All
                        => settingsModel.Modules.Modules,
                    ScreenSettingsModel.ScenarioFilterMode.Mode
                        => settingsModel.GetConfigs(View.ScenarioMode),
                    _ => throw new ArgumentOutOfRangeException()
                };
            return null;
        }
    }
}