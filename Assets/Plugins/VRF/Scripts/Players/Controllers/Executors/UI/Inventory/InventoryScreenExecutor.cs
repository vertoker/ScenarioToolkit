using System;
using System.Collections.Generic;
using System.Linq;
using SimpleUI.Core;
using VRF.BNG_Framework.Scripts.Core;
using VRF.Components.Players.Views.Player;
using VRF.Inventory.Core;
using VRF.Inventory.UI;
using VRF.Players.Controllers.Executors.UI.Inventory.Subscriptions;
using VRF.UI.GameMenu.Screens;
using Zenject;

namespace VRF.Players.Controllers.Executors.UI.Inventory
{
    public abstract class InventoryScreenExecutor<TPlayerView, TSubscription> : IInitializable, IDisposable 
        where TPlayerView : BasePlayerView
        where TSubscription : ItemSelectSubscription
    {
        protected readonly ScreensManager manager;
        protected InventoryUIController uiController;
        protected readonly TPlayerView playerView;
        protected readonly InventoryController inventoryController;
        
        private List<TSubscription> subscriptions = new();
        
        public event Action ItemSelected;

        protected abstract Grabber SpawnGrabber { get; }

        public InventoryScreenExecutor(ScreensManager manager, TPlayerView playerView, InventoryController inventoryController)
        {
            this.manager = manager;
            this.playerView = playerView;
            this.inventoryController = inventoryController;
        }
        
        public virtual void Initialize()
        {
            var inventoryScreen = manager.Find<InventoryScreen>();
            inventoryScreen.TryGetController<InventoryUIView, InventoryUIController>(false, out uiController);
            
            uiController.PageRendered += UiController_OnPageRendered;
            UiController_OnPageRendered();
        }

        public virtual void Dispose()
        {
            uiController.PageRendered -= UiController_OnPageRendered;
        }
        
        private void UiController_OnPageRendered()
        {
            Unsubscribe();
            subscriptions = uiController.ActivePageContent.Select(bind => 
                (TSubscription)Activator.CreateInstance(typeof(TSubscription), bind, (Action<PageIconBind>)SelectItem)).ToList();
            Subscribe();
        }
        
        private void Subscribe()
        {
            foreach (var subscription in subscriptions)
            {
                subscription.Initialize();
            }
        }
        
        private void Unsubscribe()
        {
            foreach (var subscription in subscriptions)
            {
                subscription.Dispose();
            }
        }
        
        private void SelectItem(PageIconBind bind)
        {
            inventoryController.SpawnItemInHand(bind.Item, playerView.ItemSpawnPoint, SpawnGrabber);
            
            ItemSelected?.Invoke();
        }
    }
}