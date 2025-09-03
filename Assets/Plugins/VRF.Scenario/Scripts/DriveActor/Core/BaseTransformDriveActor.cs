using NaughtyAttributes;
using UnityEngine;

namespace VRF.Scenario.DriveActor.Core
{
    public abstract class BaseTransformDriveActor : BaseDriveActor
    {
        [SerializeField, ReadOnly] private Transform target;
        [SerializeField] private bool useGlobal = false;
        
        public Transform Target => target;
        public bool UseGlobal => useGlobal;

        public override void OnValidate()
        {
            if (!target) target = transform;
            base.OnValidate();
        }

        public Vector3 Position
        {
            get => useGlobal ? target.position : target.localPosition;
            set
            {
                if (useGlobal)
                    target.position = value;
                else target.localPosition = value;
            }
        }
        public Vector3 EulerAngles
        {
            get => useGlobal ? target.eulerAngles : target.localEulerAngles;
            set
            {
                if (useGlobal)
                    target.eulerAngles = value;
                else target.localEulerAngles = value;
            }
        }

        public Vector3 GetPosition(Transform tr) => useGlobal ? tr.position : tr.localPosition;
        public Vector3 GetEulerAngles(Transform tr) => useGlobal ? tr.eulerAngles : tr.localEulerAngles;
        public Quaternion GetQuaternion(Transform tr) => useGlobal ? tr.rotation : tr.localRotation;
        public Vector3 GetScale(Transform tr) => useGlobal ? tr.lossyScale : tr.localScale;
    }
}