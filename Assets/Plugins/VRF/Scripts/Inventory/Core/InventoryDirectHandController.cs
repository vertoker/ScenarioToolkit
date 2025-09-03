using UnityEngine;
using VRF.Components.Items.Views;
using VRF.Components.Players.Views.Player;
using VRF.Inventory.Scriptables;
using Zenject;

namespace VRF.Inventory.Core
{
    /// <summary>
    /// Контроллер для прямого взаимодействия с предметами в руках VR (пока что) игрока
    /// </summary>
    public class InventoryDirectHandController
    {
        private readonly PlayerVRView playerVRView;
        private readonly ItemContainerPool pool;

        //TODO: GENERALIZE PlayerVRView to PlayerViewBase
        
        public InventoryDirectHandController([InjectOptional] PlayerVRView playerVRView, ItemContainerPool pool)
        {
            this.playerVRView = playerVRView;
            this.pool = pool;
        }
        
        /// <summary> Относиться ли взятый предмет к тому же типу? </summary>
        public bool IsGrabbed(InventoryItemConfig item) => GetGrabbedView(item) != null;
        public ItemView GetGrabbedView(InventoryItemConfig item)
        {
            if (!playerVRView)
                return null;
            
            var grabbable = playerVRView.RightGrabber.HeldGrabbable;
            if (grabbable == null) return null;
            
            var view = grabbable.GetComponent<ItemView>();
            if (view == null) return null;
            
            return view.ItemConfig == item ? view : null;
        }
        
        /// <summary> Создание и взятие предмета в руку </summary>
        public void DirectCreateAndGrab(InventoryItemConfig item)
        {
            if (!playerVRView)
                return;
            
            var view = pool.DequeueItem(item, false);

            var tr = view.transform;
            var point = playerVRView.ItemSpawnPoint;
            
            tr.SetParent(point);
            tr.localPosition = Vector3.zero;
            tr.localRotation = Quaternion.identity;
            
            view.OnSpawn(item);
            playerVRView.RightGrabber.GrabGrabbable(view.Grabbable);
            view.OnInitialize();
        }
        
        /// <summary> Освобождение из руки предмета и его отключение </summary>
        public void DirectReleaseAndDisable(InventoryItemConfig item)
        {
            var view = GetGrabbedView(item);
            if (view != null)
                DirectReleaseAndDisable(view);
        }
        /// <summary> Освобождение из руки предмета и его отключение </summary>
        private void DirectReleaseAndDisable(ItemView view)
        {
            if (!playerVRView)
                return;
            
            playerVRView.RightGrabber.TryRelease();
            pool.EnqueueItem(view);
        }
        
        /// <summary> Освобождение из руки предмета </summary>
        public void Release()
        {
            if (!playerVRView)
                return;
            
            var grabbed = playerVRView.RightGrabber.HeldGrabbable;
            var view = grabbed.GetComponent<ItemView>();
            playerVRView.RightGrabber.TryRelease();
            if (view != null) pool.EnqueueItem(view);
        }
    }
}