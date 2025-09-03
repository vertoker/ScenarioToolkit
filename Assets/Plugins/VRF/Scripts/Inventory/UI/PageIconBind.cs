using UnityEngine;
using VRF.Components.Items.Views;
using VRF.Inventory.Core;
using VRF.Inventory.Scriptables;

namespace VRF.Inventory.UI
{
    /// <summary>
    /// Локальный бинд экземпляра иконки, из которых состоит страница
    /// </summary>
    public readonly struct PageIconBind
    {
        public readonly InventoryItemConfig Item;
        public readonly ItemContainerPool Pool;
            
        public readonly ItemViewUI ViewUI;
            
        public PageIconBind(InventoryItemConfig item, ItemContainerPool pool, InventoryUIView view)
        {
            Item = item;
            Pool = pool;
                
            ViewUI = Pool.DequeueItemUI(Item);
                
            var trUI = ViewUI.transform;
            trUI.SetParent(view.Content);
            trUI.localPosition = Vector3.zero;
            trUI.localRotation = Quaternion.identity;
            trUI.localScale = Vector3.one;
        }

        public void Dispose()
        {
            ViewUI.Button.onClick.RemoveAllListeners();
            Pool.EnqueueItemUI(ViewUI);
        }
    }
}