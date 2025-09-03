using System;
using KBCore.Refs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRF.BNG_Framework.Scripts.Core;
using VRF.Inventory.Scriptables;
using VRF.Utils;
using VRF.Utils.Rendering.ItemRendering;
using Zenject;
using Button = UnityEngine.UI.Button;

namespace VRF.Components.Items.Views
{
    public class ItemViewUI : BaseView, IItem
    {
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text title;
        [SerializeField] private Grabbable grabbable;
        
        [SerializeField, Self] private Button button;
        [SerializeField, Self] private ButtonPointerHandler buttonPointer;

        [InjectOptional] private ItemRenderingService itemRenderingService;
        
        public Button Button => button;
        public ButtonPointerHandler ButtonPointer => buttonPointer;
        public Grabbable Grabbable => grabbable;
        
        public InventoryItemConfig ItemConfig { get; private set; }
        
        public void OnSpawn(InventoryItemConfig item)
        {
            ItemConfig = item;
            title.text = item.ItemName;
            var preview = itemRenderingService.GetItemPreview(item);
            icon.sprite = Sprite.Create(preview, new Rect(0, 0, preview.width, preview.height), new Vector2(0.5f, 0.5f));
            gameObject.SetActive(true);
        }
        public void Disable()
        {
            gameObject.SetActive(false);
            title.text = string.Empty;
            ItemConfig = null;
            DisposeInternal();
        }

        public void OnInitialize()
        {
            InitializeInternal();
        }
    }
}