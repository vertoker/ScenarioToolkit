using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Core.Scriptables;
using ScenarioToolkit.Core.World;
using ScenarioToolkit.Shared.Attributes;
using ScenarioToolkit.Shared.VRF;

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
    [ScenarioMeta("Устанавливает (не запускает) параметры для запуска сценария через Instance", 
        typeof(ScenarioLauncherInstance), typeof(StartScenarioInstance))]
    public struct SetScenarioInstance : IScenarioAction, IComponentDefaultValues, IGetScenarioLaunch
    {
        public ScenarioLauncherInstance Instance;
        public ScenarioModule Module;
        public bool UseNetwork;
        public bool UseLog;
        public PlayerIdentityConfig Identity;
        
        public ScenarioModule GetModule() => Module;
        public bool GetUseNetwork() => UseNetwork;

        public bool GetUseLog() => UseLog;

        public int GetIdentityHash() => Identity ? Identity.AssetHashCode : 0;
        public int GetIdentityHash(IdentityService identityService)
        {
            if (Identity) return Identity.AssetHashCode;
            return identityService.SelfIdentity
                ? identityService.SelfIdentity.AssetHashCode : 0;
        }
        
        public void SetDefault()
        {
            Instance = null;
            Module = null;
            UseNetwork = true;
            UseLog = false;
            Identity = null;
        }
    }
}