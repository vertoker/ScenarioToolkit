using VRF.VRBehaviours;

namespace VRF.Players.Controllers.Interaction.RaycastInteractionControllers
{
    public class ToggleSwitchInteractionController : RaycastInteractionController<ToggleSwitch>
    {
        public override void Press()
        {
            target.Toggle();
        }

        public override void Release()
        {
            
        }
    }
}