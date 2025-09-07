using Mirror;
using Scenario.Core.DataSource;
using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Core.Player;
using ScenarioToolkit.Core.Player.Roles;
using ScenarioToolkit.Core.Scriptables;
using ScenarioToolkit.Core.Serialization;
using ScenarioToolkit.Shared.VRF;
using UnityEngine;
using Zenject;

namespace ScenarioToolkit.Core.World
{
    /// <summary>
    /// Instance в мире Unity для запуска сценария не через Container. Полностью независим от других Instance,
    /// работает только в рамках сцены, где существует. Работает только на Host, у Client он просто не используется
    /// </summary>
    public class ScenarioLauncherInstance : ScenarioBehaviour, IGetScenarioLaunch, ISetScenarioLaunch
    {
        public ScenarioPlayer Player { get; private set; }
        public ScenarioLoadService LoadService { get; private set; }
        public IdentityService IdentityService { get; private set; }
        
        public bool IsConstructed { get; private set; }
        public bool IsInitialized => IsConstructed && Player.IsPlayed;

        [Header("Core")]
        [SerializeField] private ScenarioModule module;
        [SerializeField] private bool playOnStart = true;
        [SerializeField] private bool stopOnDestroy = true;
        
        [Header("Parameters")]
        [SerializeField] private bool useNetwork = false;
        [SerializeField] private bool useLog = false;
        [SerializeField] private PlayerIdentityConfig identity;
        
        public ScenarioModule GetModule() => module;
        public bool GetUseNetwork() => useNetwork;
        public bool GetNetValid() => useNetwork && NetworkServer.active;
        public bool GetUseLog() => useLog;
        public int GetIdentityHash()
        {
            if (identity) return identity.AssetHashCode;
            return IdentityService.SelfIdentity 
                ? IdentityService.SelfIdentity.AssetHashCode : 0;
        }
        public int GetIdentityHash(IdentityService identityService)
        {
            if (identity) return identity.AssetHashCode;
            return identityService.SelfIdentity 
                ? identityService.SelfIdentity.AssetHashCode : 0;
        }

        public void SetModule(ScenarioModule newModule) => module = newModule;
        public void SetUseNetwork(bool newUseNetwork) => useNetwork = newUseNetwork;
        public void SetUseLog(bool newUseLog) => useLog = newUseLog;
        public void SetPlayerIdentity(PlayerIdentityConfig newIdentity) => identity = newIdentity;

        public bool CanPlay => IsConstructed && !IsInitialized;
        public bool CanStop => IsConstructed && IsInitialized;

        [Inject]
        public void Construct(SignalBus bus, ScenarioLoadService loadService, 
            RoleFilterService filterService, IdentityService identityService)
        {
            if (IsConstructed)
            {
                Debug.LogError($"Instance is already constructed", gameObject);
                return;
            }

            IdentityService = identityService;
            LoadService = loadService;
            Player = new ScenarioPlayer(bus, loadService, filterService);
            
            IsConstructed = true;
        }

        private void Start()
        {
            if (playOnStart)
                Play();
        }
        private void OnDestroy()
        {
            if (stopOnDestroy)
                Stop();
        }

        public void ForcePlay()
        {
            if (!IsConstructed) return;
            if (IsInitialized) return;
            
            if (!TryGetModel(out var model))
            {
                Debug.LogWarning($"Can't load model");
                return;
            }
            if (!TryGetLaunchModel(out var launchModel))
            {
                Debug.LogWarning($"Can't load launch model");
                return;
            }
            
            Player.ForcePlay(model.Graph, model.Context, launchModel);
        }
        public void Play()
        {
            if (!IsConstructed) return;
            if (IsInitialized) return;
            
            if (!TryGetModel(out var model))
            {
                Debug.LogWarning($"Can't load model");
                return;
            }
            if (!TryGetLaunchModel(out var launchModel))
            {
                Debug.LogWarning($"Can't load launch model");
                return;
            }
            
            Player.Play(model.Graph, model.Context, launchModel);
        }
        public void Stop()
        {
            if (!IsConstructed) return;
            if (!IsInitialized) return;
            
            Player.Stop();
        }

        public bool TryGetModel(out IScenarioModel model)
        {
            if (!module || !module.ScenarioAsset)
            {
                model = default;
                return false;
            }
            
            var json = module.ScenarioAsset.text;
            return LoadService.TryLoadModelFromJson(json, false, out model);
        }
        public bool TryGetLaunchModel(out ScenarioLaunchModel launchModel)
        {
            launchModel = new ScenarioLaunchModel();
            if (!module || !module.IsValidIdentifier())
                return false;
            launchModel.Scenario = module.ModuleIdentifier;
            launchModel.UseNetwork = useNetwork;
            launchModel.UseLog = useLog;
            launchModel.IdentityHash = GetIdentityHash();
            return true;
        }
    }
}