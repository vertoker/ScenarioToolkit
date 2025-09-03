using UnityEngine;
using UnityEngine.InputSystem;
using VRF.Players.Controllers.Executors.Base;
using VRF.Players.Controllers.Models;
using VRF.Players.Controllers.Models.Enums;
using VRF.Utilities;

namespace VRF.Players.Controllers.Executors.Movement
{
    public class WalkExecutor : BaseMovementModelExecutor<SpeedModel>
    {
        private readonly Transform cameraPivot;
        private readonly Rigidbody playerRigidbody;
        
        public override PlayerMode ExecutableMode => PlayerMode.Idle;
        
        public WalkExecutor(SpeedModel model, Transform cameraPivot, Rigidbody playerRigidbody, 
            InputAction movement, InputAction crouch, InputAction acceleration)
            : base(movement, crouch, acceleration, model)
        {
            this.cameraPivot = cameraPivot;
            this.playerRigidbody = playerRigidbody;
        }
        
        public override void Tick()
        {
            if (!Enabled) return;
            if (MovementInput.magnitude < 0.1f) return;

            var angle = Mathf.Atan2(MovementInput.y, MovementInput.x) - Mathf.PI / 2f;

            var direction3 = cameraPivot.forward;
            direction3 = new Vector3(direction3.x, 0, direction3.z).normalized;
            direction3 = direction3.RotateVectorXZ(angle);
            
            var speed = Model.GetSpeed(IsAccelerated, IsCrouching);
            var direction2 = new Vector2(direction3.x, direction3.z);
            direction2.SetMagnitude(speed);
            //Debug.Log(string.Join('-', direction3, direction2));

            playerRigidbody.linearVelocity = playerRigidbody.linearVelocity.GetWithXZ(direction2);
            //_playerRigidbody.AddForce(direction2.x, 0, direction2.y, ForceMode.Impulse);
            //ClampVelocityXZ(_playerRigidbody, speed);
        }
    }
}