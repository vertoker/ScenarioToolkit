using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;
using VRF.Utilities;
using VRF.Utilities.Extensions;

namespace VRF.Utils.Colliders
{
    /// <summary>
    /// Простой и расширяемый utility скрипт, который позволяет скриптам извне использовать данные о том,
    /// как объект взаимодействует с коллайдерами. Имеет опциональные фильтры на
    /// тип взаимодействия, mask или tag объекта взаимодействия
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class ColliderProvider : MonoBehaviour
    {
        [SerializeField] private bool provideColliders = true;
        [SerializeField] private bool provideTriggers = false;
        
        [Space]
        [SerializeField] private bool filterByLayer = false;
        [ShowIf(nameof(filterByLayer))]
        [SerializeField] private LayerMask maskFilter = VrfLayerMask.EverythingMask;
        
        [SerializeField] private bool filterByTag = false;
        [ShowIf(nameof(filterByTag))]
        [FormerlySerializedAs("filterTag")]
        [SerializeField, Tag] private string tagFilter = string.Empty;
        
        [Space]
        [SerializeField] private bool debug = false;
        
        public event Action<Collider> TriggerEnter;
        public event Action<Collider> TriggerStay;
        public event Action<Collider> TriggerExit;
        public event Action<Collision> CollisionEnter;
        public event Action<Collision> CollisionStay;
        public event Action<Collision> CollisionExit;

        public bool ProvideTriggers
        {
            get => provideTriggers;
            set => provideTriggers = value;
        }

        public bool FilterByTag
        {
            get => filterByTag;
            set => filterByTag = value;
        }

        public string TagFilter
        {
            get => tagFilter;
            set => tagFilter = value;
        }

        private bool FilterEvents(Collider other)
        {
            if (!provideTriggers && other.isTrigger) return true;
            if (!provideColliders && !other.isTrigger) return true;
            
            var obj = other.gameObject;
            if (filterByLayer && !maskFilter.Contains(obj.layer)) return true;
            if (filterByTag && obj.CompareTag(tagFilter)) return true;
            
            return false;
        }
        
        protected virtual void OnTriggerEnter(Collider other)
        {
            if (FilterEvents(other)) return;
            TriggerEnter?.Invoke(other);
            if (debug)
                Debug.Log($"{nameof(OnTriggerEnter)}, {other.name}", other.gameObject);
        }
        protected virtual void OnTriggerStay(Collider other)
        {
            if (FilterEvents(other)) return;
            TriggerStay?.Invoke(other);
            if (debug)
                Debug.Log($"{nameof(OnTriggerStay)}, {other.name}", other.gameObject);
        }
        protected virtual void OnTriggerExit(Collider other)
        {
            if (FilterEvents(other)) return;
            TriggerExit?.Invoke(other);
            if (debug)
                Debug.Log($"{nameof(OnTriggerExit)}, {other.name}", other.gameObject);
        }

        protected virtual void OnCollisionEnter(Collision other)
        {
            if (FilterEvents(other.collider)) return;
            CollisionEnter?.Invoke(other);
            if (debug)
                Debug.Log($"{nameof(OnCollisionEnter)}, {other.gameObject.name}", other.gameObject);
        }
        protected virtual void OnCollisionStay(Collision other)
        {
            if (FilterEvents(other.collider)) return;
            CollisionStay?.Invoke(other);
            if (debug)
                Debug.Log($"{nameof(OnCollisionStay)}, {other.gameObject.name}", other.gameObject);
        }
        protected virtual void OnCollisionExit(Collision other)
        {
            if (FilterEvents(other.collider)) return;
            CollisionExit?.Invoke(other);
            if (debug)
                Debug.Log($"{nameof(OnCollisionExit)}, {other.gameObject.name}", other.gameObject);
        }

        public void Set(ColliderProvider provider)
        {
            filterByLayer = provider.filterByLayer;
            maskFilter = provider.maskFilter;
            filterByTag = provider.filterByTag;
            tagFilter = provider.tagFilter;
            debug = provider.debug;
        }
    }
}