using VRF.Components.Players.Views.NetPlayer;
using VRF.Components.Players.Views.Player;
using VRF.Players.Controllers.Scriptables;
using UnityEngine;
using System;

namespace VRF.Players.Models
{
    /// <summary>
    /// Конфигурация для спауна игрока.
    /// Используется для спауна сетевых игроков или локального игрока.
    /// Имеет 2 схемы управления, объекты для спауна в которых
    /// легко заменяются
    /// </summary>
    [Serializable]
    public class PlayerSpawnConfiguration
    {
        [SerializeField] private BasePlayerView player;
        [SerializeField] private BaseNetPlayerView netPlayer;
        [SerializeField] private BaseLocalPlayerConfig playerConfig;

        [SerializeField] private PlayerVisualConfiguration visualLocalPlayer = new(PlayerVisualMode.ControllerModels);
        [SerializeField] private PlayerVisualConfiguration visualNetworkPlayer = new(PlayerVisualMode.SkeletonIK);
        
        /// <summary> Исходный локальный игрок </summary>
        public BasePlayerView Player => player;
        /// <summary> Исходный сетевой игрок </summary>
        public BaseNetPlayerView NetPlayer => netPlayer;
        /// <summary> Настройки для локального игрока </summary>
        public BaseLocalPlayerConfig PlayerConfig => playerConfig;

        /// <summary> Исходные данные для внешнего вида локального игрока </summary>
        public PlayerVisualConfiguration VisualLocalPlayer => visualLocalPlayer;
        /// <summary> Исходные данные для внешнего вида сетевого игрока </summary>
        public PlayerVisualConfiguration VisualNetworkPlayer => visualNetworkPlayer;
    }
}