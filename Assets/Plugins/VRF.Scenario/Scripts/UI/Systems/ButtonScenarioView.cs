using System;
using SimpleUI.Templates.Base;
using UnityEngine.Events;
using VRF.Scenario.Components.Conditions;
using Zenject;

namespace VRF.Scenario.UI.Systems
{
    public class ButtonScenarioView : BaseButtonView
    {
        public override Type GetControllerType() => typeof(ButtonScenarioController);
    }
    
    public class ButtonScenarioController : BaseButtonController<ButtonScenarioView>
    {
        private readonly SignalBus _bus;

        public ButtonScenarioController(ButtonScenarioView view, SignalBus bus) : base(view)
        {
            _bus = bus;
        }

        protected override UnityAction GetAction() => FireCondition;

        private void FireCondition()
        {
            _bus.Fire(new ButtonClicked { Button = View.Button });
        }
    }
}