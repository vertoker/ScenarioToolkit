using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace VRF.Utils.Colliders
{
    /// <summary>
    /// Расширение для определения находится ли коллайдер на земле?
    /// Чтобы он работал, нужно создать отдельный объект, добавить на него коллайдер и этот скрипт
    /// и уже после поместить его под сущность так, чтобы регистрировалось только касание с землёй
    /// </summary>
    public class GroundProvider : ColliderProvider
    {
        [ShowNativeProperty]
        public bool IsGrounded { get; private set; }
        public event Action<bool> OnUpdateGround;
        
        private readonly HashSet<Collider> colliders = new();
        
        [ShowNativeProperty]
        public int CollidersCount => colliders.Count;
        
        private void OnEnable()
        {
            TriggerEnter += GroundEnter;
            TriggerExit += GroundExit;
        }
        private void OnDisable()
        {
            TriggerEnter -= GroundEnter;
            TriggerExit -= GroundExit;
        }
        
        private void GroundEnter(Collider other)
        {
            colliders.Add(other);
            TryUpdateGroundStatus(colliders.Count != 0);
        }
        private void GroundExit(Collider other)
        {
            colliders.Remove(other);
            TryUpdateGroundStatus(colliders.Count != 0);
        }

        private void TryUpdateGroundStatus(bool newStatus)
        {
            if (IsGrounded == newStatus) return;
            IsGrounded = newStatus;
            OnUpdateGround?.Invoke(IsGrounded);
        }
    }
}