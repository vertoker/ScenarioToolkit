using VRF.Components.Players.Views.Player;
using VRF.Players.Controllers.Builders;

namespace VRF.Players.Core
{
    public readonly struct PlayerControlModeData
    {
        public readonly BasePlayerView View;
        public readonly IPlayerBuilder Builder;

        public bool IsEmpty() => Builder == null;

        public PlayerControlModeData(BasePlayerView view, IPlayerBuilder builder)
        {
            View = view;
            Builder = builder;
        }
    }
}