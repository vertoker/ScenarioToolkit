using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SimpleUI.Core;
using UnityEngine;
using VRF.Components.Players;
using VRF.DataSources.Config;
using VRF.Identities.Core;
using VRF.Players.Core;
using VRF.Players.Models;
using VRF.Players.Scriptables;
using VRF.Players.Services;
using VRF.UI.Settings.Core.Interfaces;
using VRF.Utilities;
using Zenject;

namespace VRF.UI.Settings.Core
{
    public class SettingsParametersView : UIView
    {
        [SerializeField] private float saveDelay = 0.5f;
        [SerializeField] private SettingsModel defaultModel = new();

        public SettingsModel DefaultModel => defaultModel;
        public float SaveDelay => saveDelay;

        public override Type GetControllerType() => typeof(SettingsParametersController);
    }

    /// <summary>
    /// Основной контроллер настоек, администрирует загрузку/сохранение и первичную инициализацию
    /// </summary>
    public class SettingsParametersController : UIController<SettingsParametersView>, IInitializable, IDisposable
    {
        private readonly ConfigDataSource dataSource;
        private readonly IdentityService identityService;
        private readonly PlayersContainer playersContainer;
        
        public PlayerControlModes ControlModes;

        private readonly List<ISettingsParameter> parameters = new();

        public SettingsModel Model { get; private set; }
        private bool initialized;

        public SettingsParametersController(ConfigDataSource dataSource, IdentityService identityService,
            PlayersContainer playersContainer, SettingsParametersView view) : base(view)
        {
            this.dataSource = dataSource;
            this.identityService = identityService;
            this.playersContainer = playersContainer;
            playersContainer.PlayerChanged += PlayerChanged;
        }
        private void PlayerChanged()
        {
            Dispose();
            Initialize();
        }

        public void Initialize()
        {
            ControlModes = identityService.SelfIdentity.Appearance.FilterCurrentMode(playersContainer.CurrentKey.Mode);
            // Загружает или из конфига, или из стандартной модели
            Model = dataSource.Load<SettingsModel>() ?? View.DefaultModel.SerializableClone();
            initialized = true;

            // И делает первичную инициализацию
            foreach (var parameter in parameters)
                parameter.SetupModel(Model);
        }
        public void Dispose()
        {
            SaveForce();
            initialized = false;
        }

        public void Register(ISettingsParameter parameter)
        {
            parameters.Add(parameter);
            if (initialized)
                parameter.SetupModel(Model);
        }

        public void Unregister(ISettingsParameter parameter)
        {
            parameters.Remove(parameter);
        }

        private bool delayActive;

        /// <summary>
        /// Сохранение текущей модели в настройках, имеет timeout
        /// на срабатывание, поэтому можно вызывать в OnValueChanged
        /// </summary>
        public async void Save()
        {
            if (delayActive) return;
            delayActive = true;

            await UniTask.Delay(TimeSpan.FromSeconds(View.SaveDelay));

            // Чтобы не было 2 сохранения подряд при удалении контроллера
            if (!delayActive) return;
            SaveForce();
        }

        private void SaveForce()
        {
            delayActive = false;
            dataSource.Save(Model);
        }
    }
}