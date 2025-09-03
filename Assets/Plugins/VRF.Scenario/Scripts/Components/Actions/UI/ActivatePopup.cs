using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using VRF.Scenario.UI.Game.InfoTip;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Actions
{
    [ScenarioMeta("Включает этот view (через screen или gameObject)")]
    public struct ActivatePopup : IScenarioAction, IComponentDefaultValues
    {
        public InfoTipView View;
        [ScenarioMeta("Берёт текст и посылает его в шину через", typeof(SetInfo), typeof(SetInfoText))]
        public bool UseGlobalTip;
        
        public void SetDefault()
        {
            View = null;
            UseGlobalTip = true;
        }
    }
}