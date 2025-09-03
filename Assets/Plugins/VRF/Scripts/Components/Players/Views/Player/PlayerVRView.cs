using System;
using UnityEngine;
using VRF.BNG_Framework.Scripts.Core;
using VRF.BNG_Framework.Scripts.Helpers;
using VRF.Inventory.Equipment;
using VRF.Players.Checking;
using VRF.Players.Controllers.Builders;
using VRF.Players.Controllers.Scriptables;
using VRF.Players.Hands;

namespace VRF.Components.Players.Views.Player
{
    // TODO добавить для разных ролей ограничения на взаимодействия с предметами
    // TODO GENERALIZE PlayerVRView to PlayerViewBase
    
    /// <summary> Базовый конфиг для VR игрока (пока переопределений для него) не существует </summary>
    public class PlayerVRView : BasePlayerView
    {
        [Header("Hand Left")]
        [SerializeField] private HandController handLeft;
        [SerializeField] private HandSkinsController handSkinsLeft;
        [SerializeField] private Grabber grabberLeft;
        [SerializeField] private CheckingController checkingLeft;
        [Header("Hand Right")]
        [SerializeField] private HandController handRight;
        [SerializeField] private HandSkinsController handSkinsRight;
        [SerializeField] private Grabber grabberRight;
        [SerializeField] private CheckingController checkingRight;
        [Header("Equipment")]
        [SerializeField] private BeltEquipmentManager beltEquipmentManager;

        public HandController LeftHand => handLeft;
        public HandSkinsController LeftSkinsHand => handSkinsLeft;
        public Grabber LeftGrabber => grabberLeft;
        public CheckingController CheckingLeft => checkingLeft;
        
        public HandController RightHand => handRight;
        public HandSkinsController RightSkinsHand => handSkinsRight;
        public Grabber RightGrabber => grabberRight;
        public CheckingController CheckingRight => checkingRight;
        
        public BeltEquipmentManager EquipmentManager => beltEquipmentManager;
        
        protected override Type GetSelfSchemeType => typeof(SchemeVR);
        protected override Type GetControllersConfigType => typeof(PlayerVRConfig);
        public override Type GetBuilderType => typeof(PlayerVRBuilder);
    }
}