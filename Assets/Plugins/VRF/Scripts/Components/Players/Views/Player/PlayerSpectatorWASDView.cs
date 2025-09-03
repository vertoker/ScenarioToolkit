using System;
using UnityEngine;
using VRF.Players.Controllers.Builders;
using VRF.Players.Controllers.Scriptables;
using Zenject;

namespace VRF.Components.Players.Views.Player
{
    public class PlayerSpectatorWASDView : BasePlayerView
    {
        [Header("Core")]
        [SerializeField] private Rigidbody playerRigidbody;

        protected override Type GetSelfSchemeType => typeof(SchemeWASD);
        protected override Type GetControllersConfigType => typeof(PlayerSpectatorWASDConfig);
        public override Type GetBuilderType => typeof(PlayerSpectatorWASDBuilder);

        public Rigidbody PlayerRigidbody => playerRigidbody;
    }
}