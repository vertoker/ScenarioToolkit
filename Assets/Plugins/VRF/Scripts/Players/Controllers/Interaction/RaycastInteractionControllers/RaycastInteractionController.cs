namespace VRF.Players.Controllers.Interaction.RaycastInteractionControllers
{
    public abstract class RaycastInteractionController<T> : InteractionController<T>, IRaycastInteractionController
    {
        public abstract void Press();
        public abstract void Release();
    }
}