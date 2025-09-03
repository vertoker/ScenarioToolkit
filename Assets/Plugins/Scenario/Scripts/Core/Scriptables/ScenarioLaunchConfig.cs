using System;
using NaughtyAttributes;
using Scenario.Core.DataSource;
using UnityEngine;
using VRF.DataSources.Scriptables;
using VRF.Identities;
using VRF.Identities.Core;
using VRF.Players.Scriptables;

namespace Scenario.Core.Scriptables
{
    [CreateAssetMenu(fileName = nameof(ScenarioLaunchConfig), menuName = "Scenario/" + nameof(ScenarioLaunchConfig))]
    public class ScenarioLaunchConfig : ScriptableObject, IScriptableModel, IGetScenarioLaunch
    {
        [SerializeField, Expandable] private ScenarioModule module;
        [SerializeField] private bool useNetwork = true;
        [SerializeField] private bool useLog = false;
        [SerializeField] private PlayerIdentityConfig identity;
        
        public ScenarioModule GetModule() => module;
        public bool GetUseNetwork() => useNetwork;
        public bool GetUseLog() => useLog;
        public int GetIdentityHash() => identity ? identity.AssetHashCode : 0;
        public int GetIdentityHash(IdentityService identityService)
        {
            if (identity) return identity.AssetHashCode;
            return identityService.SelfIdentity
                ? identityService.SelfIdentity.AssetHashCode : 0;
        }

        public ScenarioModule FirstOrDefault(string moduleIdentifier)
        {
            if (!module) return null;
            return module.ModuleIdentifier == moduleIdentifier ? module : null;
        }

        public Type GetModelType() => typeof(ScenarioLaunchModel);
        public object GetModelObject()
        {
            var model = new ScenarioLaunchModel
            {
                Scenario = GetModule().ModuleIdentifier,
                UseNetwork = GetUseNetwork(),
                UseLog = GetUseLog(),
                IdentityHash = GetIdentityHash(),
            };
            return model;
        }
    }
}