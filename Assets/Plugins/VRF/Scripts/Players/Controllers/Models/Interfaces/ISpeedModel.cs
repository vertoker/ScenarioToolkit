using VRF.Players.Controllers.Models.Enums;

namespace VRF.Players.Controllers.Models.Interfaces
{
    public interface ISpeedModel : IPlayerModel
    {
        public ButtonPressMode CrouchBtnMode { get; }
        public ButtonPressMode AccelerationBtnMode { get; }
    }
}