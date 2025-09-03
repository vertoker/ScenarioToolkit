using Scenario.Core;
using Scenario.Core.Systems;
using Scenario.Utilities;
using VRF.BNG_Framework.Scripts.Core;
using VRF.Components.Items.Views;
using VRF.Components.Players;
using VRF.Components.Players.Views.Player;
using VRF.Inventory.Core;
using VRF.Players.Core;
using VRF.Scenario.Components.Actions;
using VRF.Scenario.Components.Actions.Items;
using VRF.Scenario.Components.Conditions;
using VRF.Scenario.Components.Conditions.Item;
using VRF.Scenario.States;
using Zenject;

namespace VRF.Scenario.Systems
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class InventorySystem : BaseScenarioStateSystem<InventoryState>
    {
        private readonly PlayersContainer playersContainer;
        private readonly ActiveItemsContainer activeItemsContainer;
        private readonly ItemContainerPool itemPool;

        public InventorySystem(SignalBus listener, PlayersContainer playersContainer,
            ActiveItemsContainer activeItemsContainer,
            InventoryController inventoryController,
            ItemContainerPool itemPool) : base(listener)
        {
            this.playersContainer = playersContainer;
            this.activeItemsContainer = activeItemsContainer;
            this.itemPool = itemPool;
            
            inventoryController.OnGrabItem += OnGrabbed;
            inventoryController.OnDropItem += OnDropped;
            
            Bus.Subscribe<AddItem>(AddItem);
            Bus.Subscribe<DeleteItem>(DeleteItem);
            Bus.Subscribe<ClearWorldItems>(ClearWorldItems);
        }

        protected override void ApplyState(InventoryState state)
        {
            foreach (var inventoryItemConfig in state.Items)
            {
                activeItemsContainer.AddItem(inventoryItemConfig);
            }
        }

        private void AddItem(AddItem component)
        {
            if (AssertLog.NotNull<AddItem>(component.ItemConfig, nameof(component.ItemConfig))) return;

            State.Items.Add(component.ItemConfig);
            
            activeItemsContainer.AddItem(component.ItemConfig);
        }

        private void DeleteItem(DeleteItem component)
        {
            if (AssertLog.NotNull<DeleteItem>(component.ItemConfig, nameof(component.ItemConfig))) return;

            State.Items.Remove(component.ItemConfig);
            
            activeItemsContainer.RemoveItem(component.ItemConfig);
        }

        private void ClearWorldItems(ClearWorldItems component)
        {
            var playerView = playersContainer.CurrentValue.View;
            
            switch (playerView)
            {
                case PlayerVRView playerVRView:
                    TryDropInventoryItem(playerVRView.LeftGrabber);
                    TryDropInventoryItem(playerVRView.RightGrabber);
                    break;
                case PlayerWASDView playerWasdView:
                    TryDropInventoryItem(playerWasdView.VirtualHandGrabber);
                    break;
            }

            itemPool.EnqueueActiveItems();
        }

        private void TryDropInventoryItem(Grabber grabber)
        {
            var grabbable = grabber.HeldGrabbable;
            if (grabbable && grabbable.TryGetComponent<ItemView>(out _))
                grabbable.DropItem(grabber);
        }

        private void OnGrabbed(IItem item)
        {
            Bus.Fire(new ItemGrabbed
            {
                ItemConfig = item.ItemConfig
            });
        }

        private void OnDropped(IItem item)
        {
            Bus.Fire(new ItemDropped
            {
                ItemConfig = item.ItemConfig
            });
        }
    }
}