using System;
using UnityEngine;
using UnityEngine.InputSystem;
using VRF.Players.Controllers.Executors.Base;
using VRF.Players.Controllers.Executors.Interfaces;
using VRF.Players.Controllers.Models;
using VRF.Players.Controllers.Models.Enums;
using VRF.Utilities;
using Zenject;

namespace VRF.Players.Controllers.Executors.Movement
{
    public class FlyHeightExecutor : BaseModelExecutor<FlyModel>, IPlayerModeExecutor, IInitializable, ITickable, IDisposable
    {
        private readonly Rigidbody rigidbody;
        private readonly InputAction acceleration;
        private readonly InputAction up, down;
        
        private bool isDown, isUp;
        private bool isAccelerated;

        public PlayerMode ExecutableMode => PlayerMode.Fly;
        
        public FlyHeightExecutor(FlyModel model, Rigidbody rigidbody,
            InputAction up, InputAction down, InputAction acceleration) : base(model)
        {
            this.rigidbody = rigidbody;
            this.up = up;
            this.down = down;
            this.acceleration = acceleration;
        }

        public override void Enable()
        {
            base.Enable();
            up.Enable();
            down.Enable();
            acceleration.Enable();
        }
        public override void Disable()
        {
            base.Disable();
            up.Disable();
            down.Disable();
            acceleration.Disable();
        }

        public void Initialize()
        {
            up.Subscribe(UpPerformed, UpCancelled);
            down.Subscribe(DownPerformed, DownCancelled);
            
            acceleration.Subscribe(Model.AccelerationBtnMode,
                AccelerationPerformed, AccelerationCancelled, AccelerationToggle);
        }
        public void Dispose()
        {
            up.Unsubscribe(UpPerformed, UpCancelled);
            down.Unsubscribe(DownPerformed, DownCancelled);
            
            acceleration.Unsubscribe(Model.AccelerationBtnMode,
                AccelerationPerformed, AccelerationCancelled, AccelerationToggle);
        }
        
        private void AccelerationPerformed(InputAction.CallbackContext ctx) => isAccelerated = true;
        private void AccelerationCancelled(InputAction.CallbackContext ctx) => isAccelerated = false;
        private void AccelerationToggle(InputAction.CallbackContext ctx) => isAccelerated = !isAccelerated;
        
        private void UpPerformed(InputAction.CallbackContext ctx) => isUp = true;
        private void UpCancelled(InputAction.CallbackContext ctx) => isUp = false;
        private void DownPerformed(InputAction.CallbackContext ctx) => isDown = true;
        private void DownCancelled(InputAction.CallbackContext ctx) => isDown = false;

        public void Tick()
        {
            if (!Enabled || !rigidbody) return;
            
            var y = isDown
                ? isUp ? 0f : Model.GetDownSpeed(isAccelerated)
                : isUp
                    ? Model.GetUpSpeed(isAccelerated)
                    : 0f;
            // TODO не работает shift и пробел вместе

            rigidbody.AddForce(0, y * Time.deltaTime, 0, ForceMode.Impulse);

            //ClampVelocityY(Model.DownSpeed, Model.UpSpeed);
            //Debug.Log(_rigidbody.velocity);
        }
        
        private void ClampVelocityY(float min, float max)
        {
            var velocity3 = rigidbody.linearVelocity;
            if (velocity3.y > max) 
                velocity3.y = max;
            else if (velocity3.y < min) 
                velocity3.y = min;
            rigidbody.linearVelocity = velocity3;
        }
    }
}