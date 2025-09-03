using System;
using SimpleUI.Core;
using UnityEngine;
using UnityEngine.UI;

namespace VRF.Inventory.UI
{
    /// <summary>
    /// View с основными данными по InventoryController
    /// </summary>
    public class InventoryUIView : UIView
    {
        [SerializeField] private int itemsPerPage = 3;
        
        [SerializeField] private Transform content;
        [SerializeField] private Button leftBtn;
        [SerializeField] private Button rightBtn;

        public int ItemsPerPage => itemsPerPage;
        
        public Transform Content => content;
        public Button LeftBtn => leftBtn;
        public Button RightBtn => rightBtn;
        
        public override Type GetControllerType() => typeof(InventoryUIController);
    }
}