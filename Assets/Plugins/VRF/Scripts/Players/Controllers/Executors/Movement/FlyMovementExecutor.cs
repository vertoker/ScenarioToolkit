using UnityEngine;
using UnityEngine.InputSystem;
using VRF.Players.Controllers.Executors.Base;
using VRF.Players.Controllers.Models;
using VRF.Players.Controllers.Models.Enums;
using VRF.Utilities;

namespace VRF.Players.Controllers.Executors.Movement
{
    public class FlyMovementExecutor : BaseMovementModelExecutor<SpeedModel>
    {
        private readonly Transform cameraTransform;
        private readonly Rigidbody rigidbody;
        
        public override PlayerMode ExecutableMode => PlayerMode.Fly;

        public FlyMovementExecutor(SpeedModel model, Transform cameraTransform, Rigidbody rigidbody, 
            InputAction movement, InputAction crouch, InputAction acceleration)
            : base(movement, crouch, acceleration, model)
        {
            this.cameraTransform = cameraTransform;
            this.rigidbody = rigidbody;
        }
        
        public override void Tick()
        {
            if (!Enabled) return;
            if (MovementInput.magnitude < 0.1f) return;
            
            var direction3 = new Vector3(MovementInput.x, 0, MovementInput.y);
            direction3 = cameraTransform.TransformDirection(direction3);
            
            var speed = Model.GetSpeed(IsAccelerated, IsCrouching);
            var direction2 = new Vector2(direction3.x, direction3.z);
            direction2.SetMagnitude(speed * Time.deltaTime);
            
            rigidbody.AddForce(direction2.x, 0, direction2.y, ForceMode.Impulse);
            ClampVelocityXZ(rigidbody, speed);
        }
    }
}