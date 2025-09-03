using KBCore.Refs;
using UnityEngine;
using VRF.Components.Players.Modules.Visuals;
using VRF.Networking.Core.Client;
using VRF.Players.Scriptables;
using Zenject;

namespace VRF.Components.Players.Modules
{
    [RequireComponent(typeof(BasePlayerVisualsModule))]
    public class AppearanceModule : BaseModule
    {
        [SerializeField, Self] private BasePlayerVisualsModule module;

        [InjectOptional] private ClientPlayerAppearances appearances;
        
        public override void Initialize()
        {
            if (appearances == null) return;
            appearances.Updated += AppearanceUpdated;
            appearances.Reset += AppearanceReset;
            
            base.Initialize();
        }
        public override void Dispose()
        {
            if (appearances == null) return;
            appearances.Updated -= AppearanceUpdated;
            appearances.Reset -= AppearanceReset;
            
            base.Dispose();
        }

        private void AppearanceUpdated(PlayerIdentityConfig config, PlayerAppearanceConfig appearance)
        {
            if (module.PlayerView.Identity == config)
                module.UpdateAppearance(appearance);
        }
        private void AppearanceReset(PlayerIdentityConfig config, PlayerAppearanceConfig appearance)
        {
            if (module.PlayerView.Identity == config)
                module.ResetAppearance();
        }
    }
}