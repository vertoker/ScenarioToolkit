using System;
using UnityEngine.InputSystem;
using VRF.BNG_Framework.Scripts.Core;

namespace VRF.Players.Controllers.Utility
{
    public class ActionForceGrabber : Grabber
    {
        private InputAction action;

        private void Awake()
        {
            action = GrabAction.action;
        }
        private void OnEnable()
        {
        }
        private void OnDisable()
        {
        }
        
    }
}