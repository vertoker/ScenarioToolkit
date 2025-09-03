using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;
using VRF.Components.Players.Views.NetPlayer;
using VRF.Components.Players.Views.Player;
using VRF.Players.Controllers.Scriptables;
using VRF.Players.Models;
using VRF.Players.Models.Player;
using VRF.Utilities;
using VRF.Utils.Identifying;

namespace VRF.Players.Scriptables
{
    /// <summary>
    /// Внешний вид игрока для всех возможных конфигураций.
    /// Есть возможность её динамически изменять у всех сетевых игроков
    /// </summary>
    [CreateAssetMenu(fileName = nameof(PlayerAppearanceConfig), menuName = "VRF/Identities/" + nameof(PlayerAppearanceConfig))]
    public class PlayerAppearanceConfig : IdentifiedScriptableObject, IControlSchemeModes
    {
        [FormerlySerializedAs("controlSchemeModes")] // Не трогать, старое название
        [SerializeField, EnumFlags] private PlayerControlModes controlModes =
            PlayerControlModes.VR & PlayerControlModes.WASD;
        
        // VR основной и самый проработанный режим управления игрока
        // Зачастую для него и создана и заточена под него большая часть кода фреймворка
        [ShowIf(nameof(IsVR))]
        [SerializeField] private PlayerSpawnConfiguration configurationVR = new();
        
        // WASD это обделённый режим управления, хотя имеет не меньшее удобство управления, чем VR
        // Тоже используется и активно дорабатывается для различных задач
        [ShowIf(nameof(IsWASD))]
        [SerializeField] private PlayerSpawnConfiguration configurationWASD = new();
        
        public bool IsVR => controlModes.ContainsFlag(PlayerControlModes.VR);
        public bool IsWASD => controlModes.ContainsFlag(PlayerControlModes.WASD);
        public PlayerSpawnConfiguration ConfigurationVR => configurationVR;
        public PlayerSpawnConfiguration ConfigurationWASD => configurationWASD;

        public PlayerSpawnConfiguration GetConfiguration(PlayerModel model)
            => GetConfigurationData(model, configuration => configuration);
        public PlayerSpawnConfiguration GetConfiguration(PlayerControlModes priorityControlMode)
            => GetConfigurationData(priorityControlMode, configuration => configuration);
        public BasePlayerView GetPlayerPrefab(PlayerModel model)
            => GetConfigurationData(model, configuration => configuration.Player);
        public BaseNetPlayerView GetNetPlayerPrefab(PlayerModel model)
            => GetConfigurationData(model, configuration => configuration.NetPlayer);
        public BaseNetPlayerView GetNetPlayerPrefab(PlayerControlModes modes)
            => GetConfigurationData(modes, configuration => configuration.NetPlayer);
        public BaseLocalPlayerConfig GetPlayerConfig(PlayerModel model)
            => GetConfigurationData(model, configuration => configuration.PlayerConfig);

        
        public TData GetConfigurationData<TData>(PlayerModel model, Func<PlayerSpawnConfiguration, TData> predicate)
        {
            var condition = GetCurrentMode(model);
            if (condition.IsVR()) return predicate(ConfigurationVR);
            if (condition.IsWASD()) return predicate(ConfigurationWASD);
            throw new NotImplementedException($"You doesn't select {controlModes}");
        }
        public TData GetConfigurationData<TData>(PlayerControlModes modes, Func<PlayerSpawnConfiguration, TData> predicate)
        {
            var currentMode = FilterCurrentMode(modes);
            if (currentMode.IsVR()) return predicate(ConfigurationVR);
            if (currentMode.IsWASD()) return predicate(ConfigurationWASD);
            throw new NotImplementedException($"You doesn't select {controlModes}");
        }
        
        public PlayerControlModes GetCurrentMode(PlayerModel model) => FilterCurrentMode(model.PriorityControlMode);
        public PlayerControlModes FilterCurrentMode(PlayerControlModes modes)
        {
            if (IsVR && IsWASD)
            {
                if (modes.IsVR()) return PlayerControlModes.VR;
                if (modes.IsWASD()) return PlayerControlModes.WASD;
            }
            if (IsVR) return PlayerControlModes.VR;
            if (IsWASD) return PlayerControlModes.WASD;
            throw new NotImplementedException($"You doesn't select {controlModes}");
        }
    }
}