using System;
using VRF.Components.Players.Views.Player;
using VRF.Networking.Messages;
using VRF.Networking.Models;
using VRF.Players.Models;
using VRF.Players.Models.Player;
using VRF.Players.Scriptables;

namespace VRF.Components.Players.Views.NetPlayer
{
    /// <summary>
    /// Представление сетевого игрока.
    /// Имеет 2 режима управления: observer и receiver
    /// </summary>
    public abstract class BaseNetPlayerView : BaseView, IControlPlayerView
    {
        public BasePlayerView PlayerView { get; private set; }
        public bool IsOwnedByLocalPlayer => PlayerView != null;
        public bool IsOwnedByServer => PlayerView == null;
        
        // Если это сетевой игрок локального игрока, то ему не нужна 
        // конфигурация для создания своего представления, так как
        // этим уже занимается локальный игрок

        public bool IsNetPlayer => true;
        public PlayerIdentityConfig Identity { get; private set; }
        public PlayerControlModes CurrentMode { get; private set; }

        public PlayerAppearanceConfig GetAppearance()
        {
            if (IsOwnedByLocalPlayer) return PlayerView.Identity.Appearance;
            if (IsOwnedByServer) return Identity.Appearance;
            return null;
        }
        
        // У сетевого игрока есть 2 вида инициализации: через Mirror и через локального игрока
        
        /// <summary>
        /// В режиме observer сетевой игрок берёт данные от локального игрока
        /// и отправляет в сеть Mirror. На каждом клиенте может быть только один такой
        /// </summary>
        /// <param name="view">Локальный view игрока</param>
        public void Initialize(BasePlayerView view)
        {
            PlayerView = view;
            Identity = view.Identity;
            CurrentMode = view.CurrentMode;
            InitializeInternal();
        }
        
        /// <summary>
        /// В режиме receiver сетевой игрок создаётся и получает данные от сети Mirror.
        /// В этом случае он становиться самостоятельным и все остальные игроки работают именно в этом режиме
        /// </summary>
        public void Initialize(PlayerIdentityConfig identityConfig, NetPlayerSpawnData playerSpawnData)
        {
            Identity = identityConfig;
            CurrentMode = playerSpawnData.CurrentMode;
            InitializeInternal();
        }
    }
}