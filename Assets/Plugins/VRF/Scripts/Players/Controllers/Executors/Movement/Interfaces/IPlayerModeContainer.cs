using System.Collections.Generic;
using VRF.Players.Controllers.Models.Enums;

namespace VRF.Players.Controllers.Executors.Movement.Interfaces
{
    public interface IPlayerModeContainer
    {
        public PlayerMode CurrentMode { get; }
        public IEnumerable<PlayerMode> AvailableModes { get; }
        
        public void SwitchNext();
        public void SwitchTo(PlayerMode mode);
    }
}