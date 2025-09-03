using VRF.Players.Controllers.Models.Enums;

namespace VRF.Players.Controllers.Executors.Interfaces
{
    public interface IPlayerModeExecutor : IModelExecutor
    {
        public PlayerMode ExecutableMode { get; }
    }
}