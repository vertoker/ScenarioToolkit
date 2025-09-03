using Scenario.Core.Systems;
using VRF.Scenario.Components.Actions;
using Zenject;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Systems
{
    public class PopupScreenSystem : BaseScenarioSystem
    {
        public PopupScreenSystem(SignalBus bus) : base(bus)
        {
            bus.Subscribe<SetPopupTitle>(SetPopupTitle);
            bus.Subscribe<SetPopupDescription>(SetPopupDescription);
        }

        private void SetPopupTitle(SetPopupTitle component)
        {
            component.Popup.TitleComponent.text = component.Title;
        }
        
        private void SetPopupDescription(SetPopupDescription component)
        {
            component.Popup.DescriptionComponent.text = component.Description;
        }
    }
}