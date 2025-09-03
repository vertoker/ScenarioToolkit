using System;
using UnityEngine;
using UnityEngine.Serialization;
using VRF.Components.Players.Modules.IK;
using VRF.Components.Players.Views;
using VRF.Components.Players.Views.NetPlayer;
using VRF.Components.Players.Views.Player;
using VRF.Components.Players.Views.PlayerIK;
using VRF.Players.Models;
using VRF.Players.Scriptables;
using VRF.Players.Services.Views;
using Zenject;

namespace VRF.Components.Players.Modules.Visuals
{
    /// <summary>
    /// Контролирующий скрипт для отображения визуальной части игрока (сетевого и локального)
    /// </summary>
    public class BasePlayerVisualsModule : BaseModule
    {
        [SerializeField] private GameObject[] models;

        public IControlPlayerView PlayerView { get; private set; }
        
        public BasePlayerIKModule ModuleIK { get; protected set; }
        [Inject] public ProjectViewSpawnerService Spawner { get; private set; }
        
        public override void Initialize()
        {
            if (View is not IControlPlayerView playerView)
                throw new ArgumentException("Visuals can be only under control by any player");
            
            PlayerView = playerView;

            if (PlayerView.IsNetPlayer && ((BaseNetPlayerView)PlayerView).IsOwnedByLocalPlayer)
            {
                // Скрипт весит на обоих игроках
                // И если сетевой игрок принадлежит локальному
                // То инициализация уже не нужна, так как
                // за неё ответственен локальный игрок
                return;
            }
            
            // Инициализация зависит от реализации конкретного класса
            InitializeAppearance(GetConfiguration());
            base.Initialize();
        }
        
        private PlayerSpawnConfiguration GetConfiguration() =>
            PlayerView.Identity.Appearance.GetConfiguration(PlayerView.CurrentMode);
        private PlayerSpawnConfiguration GetConfiguration(PlayerAppearanceConfig customAppearance) =>
            customAppearance.GetConfiguration(PlayerView.CurrentMode);
        
        protected virtual void InitializeAppearance(PlayerSpawnConfiguration spawnConfiguration) { }
        
        protected override void OnValidate()
        {
            base.OnValidate();
            SetActiveModels(false);
        }

        protected void SetActiveModels(bool active)
        {
            foreach (var model in models)
                model.SetActive(active);
        }
        
        public override void Dispose()
        {
            base.Dispose();
            if (ModuleIK) ModuleIK.SelfDestroy();
        }

        public void UpdateAppearance(PlayerAppearanceConfig appearanceConfig)
        {
            if (ModuleIK) ModuleIK.SelfDestroyDelay();
            InitializeAppearance(GetConfiguration(appearanceConfig));
        }
        public void ResetAppearance()
        {
            if (ModuleIK) ModuleIK.SelfDestroyDelay();
            InitializeAppearance(GetConfiguration());
        }
    }
}