using System;
using System.Collections.Generic;
using System.Linq;

namespace VRF.Utils.Activities
{
    /// <summary>
    /// Список bool, где для каждого из них нужно указывать своё место через порядок (order).
    /// Полученные данные формируют SortedList, который используются для их "смешивания".
    /// Итоговый результат находится в Active и равен bool, у которого самый большой order
    /// </summary>
    public class ActivityChain : IActivityContainer
    {
        private readonly bool activeByDefault;
        private readonly SortedList<int, bool> activities;
        
        public bool Active { get; private set; }
        public event Action<bool> OnUpdate;
        public event Action<bool> OnUpdateInverse;

        public ActivityChain(bool activeByDefault = false)
        {
            this.activeByDefault = activeByDefault;
            activities = new SortedList<int, bool>();
            UpdateActiveForce();
        }

        public void Add(int order, bool active)
        {
            activities.Add(order, active);
            UpdateActive();
        }
        public void Replace(int order, bool active)
        {
            activities[order] = active;
            UpdateActive();
        }
        public void Remove(int order)
        {
            activities.Remove(order);
            UpdateActive();
        }
        public void Clear()
        {
            activities.Clear();
            OnUpdate = null;
            OnUpdateInverse = null;
        }

        private void UpdateActive()
        {
            var last = Active;
            UpdateActiveNoCallback();
            if (last != Active)
                InvokeEvents();
        }
        private void UpdateActiveForce()
        {
            UpdateActiveNoCallback();
            InvokeEvents();
        }

        private void UpdateActiveNoCallback()
        {
            Active = activities.Count == 0
                ? activeByDefault 
                : activities.Last().Value;
        }
        private void InvokeEvents()
        {
            OnUpdate?.Invoke(Active);
            OnUpdateInverse?.Invoke(!Active);
        }
    }
}