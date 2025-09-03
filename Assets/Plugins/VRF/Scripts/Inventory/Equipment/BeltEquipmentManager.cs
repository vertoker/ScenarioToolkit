using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using SimpleUI.Extensions;
using UnityEngine;
using VRF.Utilities;

namespace VRF.Inventory.Equipment
{
    public class BeltEquipmentManager : MonoBehaviour
    {
        [SerializeField] private BeltSlot source;

        private readonly Dictionary<uint, BeltSlot> slots = new();
        private Transform parent;
        private bool currentActive;
        
        private void Awake()
        {
            parent = transform;
            source.gameObject.SetActive(false);
        }
        
        public void SetActive(bool newActive)
        {
            currentActive = newActive;
            foreach (var slot in slots.Values)
                slot.gameObject.SetActive(currentActive);
        }
        public void DropAll()
        {
            foreach (var slot in slots.Values)
                slot.DropItem();
        }
        
        public BeltSlot GetSlotOrDefault(uint id) => slots.GetValueOrDefault(id);
        public void InsertSlot(uint id, float height, float distance, float angle)
        {
            var slot = GetOrCreateSlot(id);
            var vec = new Vector3(Mathf.Cos(angle) * distance, height, Mathf.Sin(angle) * distance);

            slot.transform.localPosition = vec;
            slot.transform.localEulerAngles = new Vector3(0, 90 - angle * Mathf.Rad2Deg, 0);
        }
        private BeltSlot GetOrCreateSlot(uint id)
        {
            if (slots.TryGetValue(id, out var belt)) return belt;
            var newSlot = Instantiate(source, parent);
            newSlot.gameObject.SetActive(currentActive);
            slots.Add(id, newSlot);
            return newSlot;
        }
        public void DeleteSlot(uint id)
        {
            if (!slots.Remove(id, out var slot)) return;
            Destroy(slot.gameObject);
        }
    }
}