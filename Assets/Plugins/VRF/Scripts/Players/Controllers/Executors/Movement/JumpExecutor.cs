using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using VRF.Players.Controllers.Executors.Base;
using VRF.Players.Controllers.Executors.Interfaces;
using VRF.Players.Controllers.Executors.Movement.Interfaces;
using VRF.Players.Controllers.Models;
using VRF.Players.Controllers.Models.Enums;
using VRF.Utils.Colliders;
using Zenject;

namespace VRF.Players.Controllers.Executors.Movement
{
    public class JumpExecutor : BaseModelExecutor<JumpModel>, IPlayerModeExecutor, 
        IInitializable, IDisposable 
    {
        private readonly Rigidbody playerRigidbody;
        private readonly GroundProvider groundProvider;
        private readonly InputAction jump;

        private bool activeJump;
        public PlayerMode ExecutableMode => PlayerMode.Idle;

        public JumpExecutor(JumpModel model, Rigidbody playerRigidbody,
            GroundProvider groundProvider, InputAction jump) : base(model)
        {
            this.playerRigidbody = playerRigidbody;
            this.groundProvider = groundProvider;
            this.jump = jump;
        }


        public override void Enable()
        {
            base.Enable();
            jump.Enable();
        }
        public override void Disable()
        {
            base.Disable();
            jump.Disable();
        }
        
        public void Initialize()
        {
            jump.performed += Jump;
        }
        public void Dispose()
        {
            jump.performed -= Jump;
        }
        
        private async void Jump(InputAction.CallbackContext ctx)
        {
            if (!Enabled) return;
            if (!groundProvider.IsGrounded) return;
            if (activeJump) return;
            activeJump = true;
            
            var force = new Vector3(0, Model.Force, 0);
            playerRigidbody.AddForce(force, ForceMode.Impulse);

            await UniTask.Delay(TimeSpan.FromSeconds(Model.Cooldown));
            activeJump = false;
        }
    }
}