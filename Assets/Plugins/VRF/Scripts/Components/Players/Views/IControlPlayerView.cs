using VRF.Players.Models;
using VRF.Players.Models.Player;
using VRF.Players.Scriptables;

namespace VRF.Components.Players.Views
{
    public interface IControlPlayerView
    {
        public bool IsNetPlayer { get; }
        public PlayerIdentityConfig Identity { get; }
        public PlayerControlModes CurrentMode { get; }
    }
}