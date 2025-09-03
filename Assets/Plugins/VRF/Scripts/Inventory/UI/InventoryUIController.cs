using System;
using System.Collections.Generic;
using SimpleUI.Core;
using UnityEngine;
using VRF.Inventory.Core;
using VRF.Inventory.Scriptables;
using Zenject;
using IInitializable = Zenject.IInitializable;

namespace VRF.Inventory.UI
{
    /// <summary>
    /// Контроллер, отвечающий за обработку интерфейса инвентаря и прокидывания событий 
    /// </summary>
    public class InventoryUIController : UIController<InventoryUIView>, IInitializable, IDisposable
    {
        private readonly ActiveItemsContainer activeContainer;
        private readonly ItemContainerPool pool;

        public readonly List<PageIconBind> ActivePageContent;

        private int currentPage;

        public event Action PageRendered;

        public InventoryUIController([InjectOptional] ActiveItemsContainer activeContainer, 
            [InjectOptional] ItemContainerPool pool, InventoryUIView view) : base(view)
        {
            this.activeContainer = activeContainer;
            this.pool = pool;
            
            // Scene Inventory Controller not installed
            if (activeContainer == null) return;
            
            ActivePageContent = new List<PageIconBind>(view.ItemsPerPage);

            RenderPage(0);
        }
        
        public void Initialize()
        {
            if (activeContainer == null) return;
            
            View.LeftBtn.onClick.AddListener(DisplayPreviousPage);
            View.RightBtn.onClick.AddListener(DisplayNextPage);

            activeContainer.OnAddItemInInventory += AddInInventoryUI;
            activeContainer.OnRemoveItemFromInventory += RemoveFromInventoryUI;
        }
        public void Dispose()
        {
            if (activeContainer == null) return;
            
            View.LeftBtn.onClick.RemoveListener(DisplayPreviousPage);
            View.RightBtn.onClick.RemoveListener(DisplayNextPage);

            activeContainer.OnAddItemInInventory -= AddInInventoryUI;
            activeContainer.OnRemoveItemFromInventory -= RemoveFromInventoryUI;
        }
        
        /// <summary>
        /// Заменяет контент в местах размещения предметов на те предметы,
        /// которые соответствуют индексу страницы
        /// </summary>
        private void RenderPage(int page)
        {
            foreach (var view in ActivePageContent)
            {
                view.Dispose();
            }

            currentPage = page;
            ActivePageContent.Clear();
            
            var startIndex = page * View.ItemsPerPage;
            var endIndex = Mathf.Min(startIndex + View.ItemsPerPage, activeContainer.Items.Count);
            
            for (var i = startIndex; i < endIndex; i++)
            {
                var item = activeContainer.Items[i];
                var bind = new PageIconBind(item, pool, View);
                ActivePageContent.Add(bind);
            }
            
            RenderButtons();
            
            PageRendered?.Invoke();
        }

        private void RenderButtons()
        {
            var maxPagesCount = Mathf.CeilToInt(activeContainer.Items.Count / (float)View.ItemsPerPage);
            View.LeftBtn.gameObject.SetActive(currentPage > 0);
            View.RightBtn.gameObject.SetActive(currentPage < maxPagesCount - 1);
        }
        
        private void DisplayNextPage() => RenderPage(currentPage + 1);
        private void DisplayPreviousPage() => RenderPage(currentPage - 1);
        
        private void AddInInventoryUI(InventoryItemConfig inventoryItem) => RenderPage(currentPage);
        private void RemoveFromInventoryUI(InventoryItemConfig inventoryItem) => RenderPage(currentPage);
    }
}