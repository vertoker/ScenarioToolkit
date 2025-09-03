using System;
using UnityEngine.InputSystem;
using VRF.BNG_Framework.Scripts.Core;
using Zenject;

namespace VRF.Players.Controllers.Executors.Utility
{
    public class TeleportExecutor : IInitializable, IDisposable, ILateTickable
    {
        private readonly PlayerTeleport playerTeleport;
        private readonly InputAction teleportAction;

        public TeleportExecutor(PlayerTeleport playerTeleport, InputAction teleportAction)
        {
            this.playerTeleport = playerTeleport;
            this.teleportAction = teleportAction;
        }

        public void Initialize()
        {
            teleportAction.performed += OnStartTeleport;
            teleportAction.canceled += OnFinishTeleport;
            
            teleportAction.Enable();
            playerTeleport.EnableTeleportation();
        }
        public void Dispose()
        {
            playerTeleport.DisableTeleportation();
            teleportAction.Disable();
            
            teleportAction.performed -= OnStartTeleport;
            teleportAction.canceled -= OnFinishTeleport;
        }

        private void OnStartTeleport(InputAction.CallbackContext ctx)
        {
            playerTeleport.aimingTeleport = true;
        }
        private void OnFinishTeleport(InputAction.CallbackContext ctx)
        {
            playerTeleport.aimingTeleport = false;
        }

        public void LateTick()
        {
            if(!playerTeleport.teleportationEnabled) return;
            if (playerTeleport.aimingTeleport) playerTeleport.DoCheckTeleport();
            else if (playerTeleport.KeyUpFromTeleport()) playerTeleport.TryOrHideTeleport();
            if (playerTeleport.aimingTeleport) playerTeleport.calculateParabola(playerTeleport.RightTeleportTransform);
        }
    }
}