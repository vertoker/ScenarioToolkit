using System;
using System.Collections.Generic;
using VRF.Components.Players.Views.NetPlayer;

namespace VRF.Networking.Players
{
    /// <summary>
    /// Контейнер созданных и проинициализированных игроков.
    /// Добавление идёт из самих net игроков, так как инициализация
    /// происходит в них же
    /// </summary>
    public class NetPlayersContainerService
    {
        private readonly HashSet<BaseNetPlayerView> netPlayers = new();

        public event Action<BaseNetPlayerView> OnAddPlayer;
        public event Action<BaseNetPlayerView> OnRemovePlayer;

        public IReadOnlyCollection<BaseNetPlayerView> NetPlayers => netPlayers;
        
        public bool Add(BaseNetPlayerView newPlayer)
        {
            var result = netPlayers.Add(newPlayer);
            if (result) OnAddPlayer?.Invoke(newPlayer);
            return result;
        }
        public bool Remove(BaseNetPlayerView newPlayer)
        {
            var result = netPlayers.Remove(newPlayer);
            if (result) OnRemovePlayer?.Invoke(newPlayer);
            return result;
        }
        
        public bool CanSpectate() => NetPlayers.Count > 1;
        public bool CanSwitchSpectatePlayers() => NetPlayers.Count > 2;
    }
}