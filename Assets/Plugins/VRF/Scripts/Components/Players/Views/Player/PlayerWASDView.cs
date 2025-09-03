using System;
using UnityEngine;
using VRF.BNG_Framework.Scripts.Core;
using VRF.Players.Checking;
using VRF.Players.Controllers.Builders;
using VRF.Players.Controllers.Scriptables;
using VRF.Players.Raycasting;
using Zenject;

namespace VRF.Components.Players.Views.Player
{
    public class PlayerWASDView : BasePlayerView
    {
        [Header("Core")]
        [SerializeField] private Transform playerTransform;
        [SerializeField] private Rigidbody playerRigidbody;
        [SerializeField] private CapsuleCollider playerCollider;
        [Header("Interact")]
        [SerializeField] private CheckingController checkingController;
        [SerializeField] private Grabber virtualHandGrabber;
        [SerializeField] private PhysicsRaycast raycast;
        
        public Transform PlayerTransform => playerTransform;
        public Rigidbody PlayerRigidbody => playerRigidbody;
        public CapsuleCollider PlayerCollider => playerCollider;
        
        public CheckingController CheckingController => checkingController;
        public Grabber VirtualHandGrabber => virtualHandGrabber;
        public PhysicsRaycast Raycast => raycast;

        protected override Type GetSelfSchemeType => typeof(SchemeWASD);
        protected override Type GetControllersConfigType => typeof(PlayerWASDConfig);
        public override Type GetBuilderType => typeof(PlayerWASDBuilder);
    }
}
