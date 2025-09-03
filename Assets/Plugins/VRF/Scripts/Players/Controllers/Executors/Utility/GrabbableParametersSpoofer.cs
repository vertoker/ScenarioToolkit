using VRF.BNG_Framework.Scripts.Core;
using VRF.Players.Controllers.Models;

namespace VRF.Players.Controllers.Executors.Utility
{
    public class GrabbableParametersSpoofer
    {
        private readonly VirtualHandModel model;

        private bool instantMovement;
        private float velocityForce;
        private float angularVelocityForce;
        private float throwForceMultiplier;
        private float throwForceMultiplierAngular;

        public GrabbableParametersSpoofer(VirtualHandModel model)
        {
            this.model = model;
        }

        public void Enable(Grabbable grabbable)
        {
            if (grabbable == null) return;
            instantMovement = grabbable.InstantMovement;
            grabbable.InstantMovement = true;

            if (model.OverrideVelocityForce)
            {
                velocityForce = grabbable.MoveVelocityForce;
                grabbable.MoveVelocityForce = model.VelocityForce;
            }
            if (model.OverrideAngularVelocityForce)
            {
                angularVelocityForce = grabbable.MoveAngularVelocityForce;
                grabbable.MoveAngularVelocityForce = model.AngularVelocityForce;
            }

            if (model.OverrideThrowForceMultiplier)
            {
                throwForceMultiplier = grabbable.ThrowForceMultiplier;
                grabbable.ThrowForceMultiplier = model.ThrowForceMultiplier;
            }
            if (model.OverrideThrowForceMultiplierAngular)
            {
                throwForceMultiplierAngular = grabbable.ThrowForceMultiplierAngular;
                grabbable.ThrowForceMultiplierAngular = model.ThrowForceMultiplierAngular;
            }
        }
        public void Disable(Grabbable grabbable)
        {
            if (grabbable == null) return;
            grabbable.InstantMovement = instantMovement;
            
            if (model.OverrideVelocityForce)
                grabbable.MoveVelocityForce = velocityForce;
            if (model.OverrideAngularVelocityForce)
                grabbable.MoveAngularVelocityForce = angularVelocityForce;
            
            if (model.OverrideThrowForceMultiplier)
                grabbable.ThrowForceMultiplier = throwForceMultiplier;
            if (model.OverrideThrowForceMultiplierAngular)
                grabbable.ThrowForceMultiplierAngular = throwForceMultiplierAngular;
        }
    }
}