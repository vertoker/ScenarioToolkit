using System;
using SimpleUI.Core;
using VRF.Components.Players;
using VRF.Players.Core;
using VRF.Players.Scriptables;
using VRF.Players.Services;

namespace VRF.UI.Settings.Core
{
    public class SettingsEnabledView : BaseParameterView
    {
        public override Type GetControllerType() => typeof(SettingsEnabledController);
    }
    
    public class SettingsEnabledController : UIController<SettingsEnabledView>
    {
        public SettingsEnabledController(PlayerIdentityConfig identity, PlayersContainer playersContainer,
            SettingsEnabledView view) : base(view)
        {
            var controlModes = identity.Appearance.FilterCurrentMode(playersContainer.CurrentKey.Mode);
            view.SetEnabled(controlModes);
        }
    }
}