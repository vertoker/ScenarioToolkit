using Scenario.Core;
using Scenario.Core.Systems;
using Scenario.Utilities;
using VRF.Scenario.Components.Actions;
using Zenject;

namespace VRF.Scenario.Systems
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class PopupActivateSystem : BaseScenarioSystem
    {
        public PopupActivateSystem(SignalBus bus) : base(bus)
        {
            Bus.Subscribe<ActivatePopup>(ActivatePopup);
        }
        
        private void ActivatePopup(ActivatePopup component)
        {
            if (AssertLog.NotNull<ActivatePopup>(component.View, nameof(component.View))) return;
            
            if (component.View.Screen)
                component.View.Screen.Open();
            else component.View.gameObject.SetActive(true);
            
            if (component.UseGlobalTip)
            {
                if (component.View.IsImage) // Text + Image
                {
                    Bus.Fire(new SetInfo
                    {
                        Text = component.View.TextComponent?.text,
                        Sprite = component.View.ImageComponent?.sprite
                    });
                }
                else if (component.View.IsText)
                {
                    Bus.Fire(new SetInfoText { Text = component.View.TextComponent?.text });
                }
            }
        }
    }
}