using System;
using JetBrains.Annotations;
using UnityEngine;
using VRF.Components.Players;
using VRF.Components.Players.Modules.Visuals;
using VRF.Components.Players.Views.Player;
using VRF.Identities;
using VRF.Identities.Core;
using VRF.Networking.Core.Client;
using VRF.Players.Core;
using VRF.Players.Models;
using VRF.Players.Scriptables;
using Zenject;

namespace VRF.Players.Services.Appearances
{
    public class AppearanceService : IInitializable, IDisposable
    {
        private readonly PlayersContainer playersContainer;
        [CanBeNull] private readonly ClientPlayerAppearances appearances;
        [CanBeNull] private readonly PlayerAppearanceConfig overrideAppearance;
        [CanBeNull] private readonly IdentityService identityService;

        private PlayerAppearanceConfig currentAppearance;
        
        public AppearanceService(PlayersContainer playersContainer, 
            [InjectOptional] PlayerAppearanceConfig overrideAppearance,
            [InjectOptional] ClientPlayerAppearances appearances, 
            [InjectOptional] IdentityService identityService)
        {
            this.appearances = appearances;
            this.playersContainer = playersContainer;
            this.overrideAppearance = overrideAppearance;
            this.identityService = identityService;
        }

        public void Initialize()
        {
            playersContainer.PlayerChanged += PlayerChanged;
            
            if (overrideAppearance)
                UpdateAppearance(overrideAppearance);
        }

        public void Dispose()
        {
            playersContainer.PlayerChanged -= PlayerChanged;
            
            if (overrideAppearance)
                ResetAppearance();
        }

        private void PlayerChanged()
        {
            if (currentAppearance)
            {
                UpdateAppearance(currentAppearance);
            }
            else
            {
                ResetAppearance();
            }
        }

        public void UpdateAppearance(PlayerAppearanceConfig appearance)
        {
            if (!appearance)
            {
                Debug.LogWarning($"Empty appearance, drop");
                return;
            }

            currentAppearance = appearance;
            
            if (identityService != null)
                appearances?.SendUpdateAppearance(identityService.SelfIdentity, appearance);
            else playersContainer.CurrentValue.View.GetComponent<BasePlayerVisualsModule>().UpdateAppearance(appearance);
        }
        public void ResetAppearance()
        {
            currentAppearance = null;
            
            if (identityService != null)
                appearances?.SendResetAppearance(identityService.SelfIdentity);
            else playersContainer.CurrentValue.View.GetComponent<BasePlayerVisualsModule>().ResetAppearance();
        }
    }
}