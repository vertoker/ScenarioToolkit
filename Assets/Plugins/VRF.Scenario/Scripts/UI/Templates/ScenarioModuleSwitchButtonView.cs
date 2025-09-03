using System;
using Scenario.Core.Scriptables;
using SimpleUI.Templates.Base;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using VRF.Scenario.Services;

namespace VRF.Scenario.UI.Templates
{
    public class ScenarioModuleSwitchButtonView : BaseButtonView
    {
        [Space]
        [SerializeField] private ScenarioModule module;
        [SerializeField] private ScenarioModuleLoader.ModeParameters loadParameters = new();

        public ScenarioModule Module
        {
            get => module;
            set => module = value;
        }
        public ScenarioModuleLoader.ModeParameters LoadParameters => loadParameters;

        public override Type GetControllerType() => typeof(ScenarioModuleSwitchButtonController);
    }

    public class ScenarioModuleSwitchButtonController : BaseButtonController<ScenarioModuleSwitchButtonView>
    {
        private readonly ScenarioModuleLoader loader;

        public ScenarioModuleSwitchButtonController(ScenarioModuleLoader loader, 
            ScenarioModuleSwitchButtonView view) : base(view)
        {
            this.loader = loader;
        }

        protected override UnityAction GetAction() => LoadModule;

        private void LoadModule()
        {
            loader.Load(View.Module, View.LoadParameters, View.gameObject);
        }
    }
}