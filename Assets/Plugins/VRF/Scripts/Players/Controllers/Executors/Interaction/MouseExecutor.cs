using System;
using UnityEngine;
using UnityEngine.InputSystem;
using VRF.Players.Controllers.Executors.Base;
using VRF.Players.Controllers.Models;
using VRF.Players.Services.Settings;
using VRF.Utilities;
using Zenject;

namespace VRF.Players.Controllers.Executors.Interaction
{
    public class MouseExecutor : BaseModelExecutor<MouseModel>, ITickable, IInitializable, IDisposable
    {
        private readonly Transform cameraTransform;
        private readonly MouseSensitivityParameter mouseSensitivity;

        private readonly InputAction delta;
        private readonly InputAction rotationModeAction;
        private Vector2 playerLook;
        private bool rotationMode;
        
        private float rotationX, rotationY;
        private Quaternion originalRotation;
        
        public MouseExecutor(MouseModel model, Transform cameraTransform, 
            MouseSensitivityParameter mouseSensitivity, InputAction delta, 
            InputAction rotationModeAction = null) : base(model)
        {
            this.cameraTransform = cameraTransform;
            this.mouseSensitivity = mouseSensitivity;
            this.delta = delta;
            this.rotationModeAction = rotationModeAction;
        }

        public override void Enable()
        {
            base.Enable();
            delta.Enable();
        }
        public override void Disable()
        {
            base.Disable();
            delta.Disable();
        }

        public void Initialize()
        {
            delta.Subscribe(DeltaPerformed, DeltaCancelled);
            rotationModeAction?.Subscribe(RotationModePerformed, RotationModeCanceled);
        }
        public void Dispose()
        {
            delta.Unsubscribe(DeltaPerformed, DeltaCancelled);
            rotationModeAction?.Unsubscribe(RotationModePerformed, RotationModeCanceled);
        }
        
        private void DeltaPerformed(InputAction.CallbackContext ctx) => playerLook = ctx.ReadValue<Vector2>();
        private void DeltaCancelled(InputAction.CallbackContext ctx) => playerLook = Vector2.zero;
        private void RotationModePerformed(InputAction.CallbackContext ctx) => rotationMode = true;
        private void RotationModeCanceled(InputAction.CallbackContext ctx) => rotationMode = false;

        public void Tick()
        {
            if (Enabled && !rotationMode && cameraTransform)
            {
                /*var mouseX = _playerLook.x * _mouseSensitivityParameter.Value.x * Time.deltaTime;
                var mouseY = _playerLook.y * _mouseSensitivityParameter.Value.y * Time.deltaTime;
                _rotationY -= mouseY;
                _rotationY = Mathf.Clamp(_rotationY, -Config.Mouse.Clamp.x, Config.Mouse.Clamp.y);
                View.CameraPivot.localRotation = Quaternion.Euler(_rotationY, 0f, 0f);
                View.CameraPivotAdditional.Rotate(Vector3.up * mouseX);*/
                
                
                rotationX += playerLook.x * mouseSensitivity.Value.x * Time.deltaTime;
                rotationY += playerLook.y * mouseSensitivity.Value.y * Time.deltaTime;
                //_rotationX = ClampAngle(_rotationX, clampX.x, clampX.y);
                rotationY = ClampAngle(rotationY, Model.Clamp.x, Model.Clamp.y);
                var xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
                var yQuaternion = Quaternion.AngleAxis(rotationY, -Vector3.right);
                cameraTransform.localRotation = /*_originalRotation * */xQuaternion * yQuaternion;
            }
        }
        
        private static float ClampAngle(float angle, float min, float max)
        {
            switch (angle)
            {
                case < -360F:
                    angle += 360F;
                    break;
                case > 360F:
                    angle -= 360F;
                    break;
            }
            return Mathf.Clamp(angle, min, max);
        }
    }
}