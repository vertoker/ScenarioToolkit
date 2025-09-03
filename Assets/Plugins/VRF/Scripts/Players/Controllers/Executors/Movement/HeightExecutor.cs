using System;
using UnityEngine;
using UnityEngine.InputSystem;
using VRF.Players.Controllers.Executors.Base;
using VRF.Players.Controllers.Executors.Interfaces;
using VRF.Players.Controllers.Executors.Movement.Interfaces;
using VRF.Players.Controllers.Models;
using VRF.Players.Controllers.Models.Enums;
using VRF.Utilities;
using Zenject;

namespace VRF.Players.Controllers.Executors.Movement
{
    public class HeightExecutor : BaseModelExecutor<HeightModel>, IPlayerModeExecutor,
        IInitializable, IDisposable, ILateTickable
    {
        private readonly CapsuleCollider playerCollider;
        private readonly Transform cameraPivot;
        private readonly InputAction crouch;

        private bool isCrouching;
        private float currentHeightDown;

        public PlayerMode ExecutableMode => PlayerMode.Idle;

        public HeightExecutor(HeightModel model, CapsuleCollider playerCollider, Transform cameraPivot,
            InputAction crouch) : base(model)
        {
            this.playerCollider = playerCollider;
            this.cameraPivot = cameraPivot;
            this.crouch = crouch;
        }

        public override void Enable()
        {
            base.Enable();
            crouch.Enable();
        }

        public override void Disable()
        {
            base.Disable();
            crouch.Disable();
        }

        public void Initialize()
        {
            crouch.Subscribe(Model.CrouchBtnMode,
                CrouchPerformed, CrouchCancelled, CrouchToggle);
            currentHeightDown = Model.GetHeightDown(isCrouching);
        }

        public void Dispose()
        {
            crouch.Unsubscribe(Model.CrouchBtnMode,
                CrouchPerformed, CrouchCancelled, CrouchToggle);
        }

        private void CrouchPerformed(InputAction.CallbackContext ctx) => isCrouching = true;
        private void CrouchCancelled(InputAction.CallbackContext ctx) => isCrouching = false;
        private void CrouchToggle(InputAction.CallbackContext ctx) => isCrouching = !isCrouching;

        private void UpdateCollider()
        {
            var targetHeightDown = Model.GetHeightDown(isCrouching);

            var diff = targetHeightDown - currentHeightDown;

            if (diff > Model.HeightEpsilon)
                currentHeightDown = Mathf.MoveTowards(currentHeightDown, targetHeightDown, Model.StandUpSpeed);
            else if (diff < -Model.HeightEpsilon)
                currentHeightDown = Mathf.MoveTowards(currentHeightDown, targetHeightDown, Model.SitDownSpeed);
            
            playerCollider.height = currentHeightDown + Model.HeightUp;
            playerCollider.center = new Vector3(0, playerCollider.height / 2f, 0);
            cameraPivot.localPosition = new Vector3(0, playerCollider.height, 0);
        }
        
        public void LateTick()
        {
            if (!Enabled) return;
            UpdateCollider();
        }
    }
}