using KBCore.Refs;
using UnityEngine;
using VRF.BNG_Framework.Scripts.Core;
using VRF.Components.Items.Views;

namespace VRF.Inventory.Equipment
{
    public class BeltSlot : MonoBehaviour
    {
        [Self, SerializeField] private SnapZone snapZone;

        private Grabbable slotGrabbable;
        
        public bool HasItem() => snapZone.HeldItem;
        public Grabbable GetItem() => snapZone.HeldItem;
        
        public void AddItem(ItemView itemView)
        {
            slotGrabbable = itemView.Grabbable;
            snapZone.GrabGrabbable(slotGrabbable);
            slotGrabbable.OnDropped += ReturnToSlot;
        }

        public void AddItem(Grabbable grabbable)
        {
            slotGrabbable = grabbable;
            snapZone.GrabGrabbable(grabbable);
            slotGrabbable.OnDropped += ReturnToSlot;
        }

        public void DropItem()
        {
            slotGrabbable.OnDropped -= ReturnToSlot;
            snapZone.ReleaseAll();
        }

        public void DestroyItem()
        {
            slotGrabbable.OnDropped -= ReturnToSlot;

            var heldItem = snapZone.HeldItem;
            DropItem();
            Destroy(heldItem.gameObject);
        }

        private void OnDisable()
        {
            DropItem();
        }

        private void OnDestroy()
        {
            DropItem();
        }

        private void ReturnToSlot()
        {
            slotGrabbable.OnDropped -= ReturnToSlot;
            AddItem(slotGrabbable);
        }
    }
}