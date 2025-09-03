namespace VRF.Players.Raycasting
{
    public class Raycastable : BaseRaycastable
    {
        private PhysicsRaycast currentRaycaster;
        public bool IsSelected => currentRaycaster && (Raycastable)currentRaycaster.CurrentRaycastable == this;

        public override void OnHoverStart(PhysicsRaycast raycaster)
        {
            currentRaycaster = raycaster;
            base.OnHoverStart(raycaster);
        }
        public override void OnHoverStay(PhysicsRaycast raycaster)
        {
            currentRaycaster = raycaster;
            base.OnHoverStay(raycaster);
        }
        public override void OnHoverEnd(PhysicsRaycast raycaster)
        {
            currentRaycaster = raycaster;
            base.OnHoverEnd(raycaster);
        }
        
        public override void OnButtonPress(PhysicsRaycast raycaster)
        {
            currentRaycaster = raycaster;
            base.OnButtonPress(raycaster);
        }
        public override void OnButtonStay(PhysicsRaycast raycaster)
        {
            currentRaycaster = raycaster;
            base.OnButtonStay(raycaster);
        }
        public override void OnButtonRelease(PhysicsRaycast raycaster)
        {
            currentRaycaster = raycaster;
            base.OnButtonRelease(raycaster);
        }
    }
}