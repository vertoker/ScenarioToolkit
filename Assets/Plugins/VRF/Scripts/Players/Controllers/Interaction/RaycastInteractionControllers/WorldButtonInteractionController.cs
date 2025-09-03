using VRF.VRBehaviours;

namespace VRF.Players.Controllers.Interaction.RaycastInteractionControllers
{
    public class WorldButtonInteractionController : RaycastInteractionController<WorldButton>
    {
        public override void Press()
        {
            target.Press();
        }

        public override void Release()
        {
            target.Release();
        }
    }
}