using System;
using System.Collections.Generic;
using System.Linq;
using KBCore.Refs;
using UnityEngine;
using VRF.BNG_Framework.Scripts.Core;
using VRF.Inventory.Scriptables;
using VRF.Utilities.Extensions;

namespace VRF.Components.Items.Views
{
    [RequireComponent(typeof(Grabbable))]
    public class ItemView : BaseView, IItem
    {
        [SerializeField, Self] private Grabbable grabbable;
        public Grabbable Grabbable => grabbable;

        public InventoryItemConfig ItemConfig { get; private set; }

        public void OnSpawn(InventoryItemConfig item)
        {
            ItemConfig = item;
            gameObject.SetActive(true);
        }
        public void OnInitialize()
        {
            InitializeInternal();
        }
        
        public void Disable()
        {
            gameObject.SetActive(false);
            //ItemConfig = null;
            DisposeInternal();
        }

#if UNITY_EDITOR
        public void OnDrawGizmosSelected()
        {
            var renderers = GetComponentsInChildren<Renderer>();
            if(renderers.Length <= 0)
                return;
            var bounds = new Bounds(renderers[0].localBounds.center, Vector3.zero);
            foreach (var rend in renderers)
            {
                var rendTransform = rend.transform;
                var rendLocalBounds = rend.localBounds;
                var center = rendTransform.position + rendTransform.TransformVector(rendLocalBounds.center);
                var points = rendLocalBounds.GetCorners(false).Select(corner =>
                    transform.InverseTransformPoint(center + rendTransform.TransformVector(corner)));

                foreach (var point in points)
                {
                    bounds.Encapsulate(point);
                }
            }

            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(bounds.center, bounds.size);
        }
#endif
    }
}