using System;
using UnityEngine;

namespace VRF.Scenario.DriveActor.Core
{
    public abstract class BaseDriveActor : MonoBehaviour
    {
        public const float OneEpsilon = 0.9999999f;
        public const float EpsilonEqual = 0.0005f;
        
        [SerializeField, Range(0, 1)] private float value;
        public event Action<BaseDriveActor, float> ValueUpdated;

        public float Value => value;

        public virtual void OnValidate()
        {
            UpdateValue();
            ValueUpdated?.Invoke(this, value);
        }
        
        public void SetValue(float newValue)
        {
            value = Mathf.Repeat(newValue, 1);
            UpdateValue();
            ValueUpdated?.Invoke(this, value);
        }

        protected abstract void UpdateValue();
    }
}