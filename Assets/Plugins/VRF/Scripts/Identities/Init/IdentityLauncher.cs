using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using VRF.DataSources;
using VRF.Identities.Core;
using VRF.Identities.Models;
using Zenject;

namespace VRF.Identities.Init
{
    public class IdentityLauncher : IInitializable, IDisposable
    {
        private readonly ContainerDataSource dataSource;
        [CanBeNull] private readonly IdentityService identityService;
        private readonly DataSourceType[] identitySources;
        private readonly bool initialize, dispose;

        public IdentityLauncher(ContainerDataSource dataSource, [InjectOptional] IdentityService identityService,
            DataSourceType[] identitySources, bool initialize, bool dispose)
        {
            this.dataSource = dataSource;
            this.identityService = identityService;
            this.identitySources = identitySources;
            this.initialize = initialize;
            this.dispose = dispose;
        }
        
        public void Initialize()
        {
            if (initialize)
                InitIdentity(identitySources);
        }
        public void Dispose()
        {
            if (dispose)
                ClearIdentity();
        }
        
        public void InitIdentity(IEnumerable<DataSourceType> localIdentitySources)
        {
            if (identityService == null) return;

            var identityConfig = dataSource.Load<AuthIdentityModel>(localIdentitySources);

            if (identityConfig == null)
            {
                Debug.LogWarning($"Empty <b>{nameof(AuthIdentityModel)}</b>, abort identity initialization");
                return;
            }

            if (identityService.TryInitializeSelfIdentity(identityConfig))
                Debug.Log($"<b>Init</b> Self Identity " +
                          $"id=<b>{identityConfig.IdentityID}</b>, " +
                          $"name=<b>{identityConfig.IdentityName}</b>");
        }
        public void ClearIdentity()
        {
            identityService?.ClearSelfIdentity();
        }
    }
}