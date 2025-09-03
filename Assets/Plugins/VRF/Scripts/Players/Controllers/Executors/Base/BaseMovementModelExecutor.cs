using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using VRF.Players.Controllers.Executors.Interfaces;
using VRF.Players.Controllers.Models.Enums;
using VRF.Players.Controllers.Models.Interfaces;
using VRF.Utilities;
using Zenject;

namespace VRF.Players.Controllers.Executors.Base
{
    public abstract class BaseMovementModelExecutor<TModel> : BaseModelExecutor<TModel>, 
        IPlayerModeExecutor, IInitializable, ITickable, IDisposable where TModel : ISpeedModel
    {
        private readonly InputAction movement;
        private readonly InputAction crouch;
        private readonly InputAction acceleration;

        protected Vector2 MovementInput;
        protected bool IsCrouching;
        protected bool IsAccelerated;
        
        public abstract PlayerMode ExecutableMode { get; }
        
        protected BaseMovementModelExecutor(InputAction movement, 
            [CanBeNull] InputAction crouch, 
            [CanBeNull] InputAction acceleration, 
            TModel model) : base(model)
        {
            this.movement = movement;
            this.crouch = crouch;
            this.acceleration = acceleration;
        }
        
        public override void Enable()
        {
            base.Enable();
            movement.Enable();
            crouch?.Enable();
            acceleration?.Enable();
        }
        public override void Disable()
        {
            base.Disable();
            movement.Disable();
            crouch?.Disable();
            acceleration?.Disable();
        }
        
        public virtual void Initialize()
        {
            movement.Subscribe(MovementPerformed, MovementCancelled);
            
            acceleration?.Subscribe(Model.AccelerationBtnMode,
                AccelerationPerformed, AccelerationCancelled, AccelerationToggle);
            crouch?.Subscribe(Model.CrouchBtnMode,
                CrouchPerformed, CrouchCancelled, CrouchToggle);
        }
        public virtual void Dispose()
        {
            movement.Unsubscribe(MovementPerformed, MovementCancelled);
            
            acceleration?.Unsubscribe(Model.AccelerationBtnMode,
                AccelerationPerformed, AccelerationCancelled, AccelerationToggle);
            crouch?.Unsubscribe(Model.CrouchBtnMode,
                CrouchPerformed, CrouchCancelled, CrouchToggle);
        }
        
        private void MovementPerformed(InputAction.CallbackContext ctx) => MovementInput = ctx.ReadValue<Vector2>();
        private void MovementCancelled(InputAction.CallbackContext ctx) => MovementInput = Vector2.zero;
        
        private void AccelerationPerformed(InputAction.CallbackContext ctx) => IsAccelerated = true;
        private void AccelerationCancelled(InputAction.CallbackContext ctx) => IsAccelerated = false;
        private void AccelerationToggle(InputAction.CallbackContext ctx) => IsAccelerated = !IsAccelerated;
        
        private void CrouchPerformed(InputAction.CallbackContext ctx) => IsCrouching = true;
        private void CrouchCancelled(InputAction.CallbackContext ctx) => IsCrouching = false;
        private void CrouchToggle(InputAction.CallbackContext ctx) => IsCrouching = !IsCrouching;
        
        public abstract void Tick();
        
        protected static void ClampVelocityXZ(Rigidbody rb, float speed)
        {
            var velocity3 = rb.linearVelocity;
            var velocity2 = new Vector2(velocity3.x, velocity3.z);
            velocity2 = Vector2.ClampMagnitude(velocity2, speed);
            rb.linearVelocity = new Vector3(velocity2.x, velocity3.y, velocity2.y);
        }
    }
}