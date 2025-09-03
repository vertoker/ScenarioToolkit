using KBCore.Refs;
using UnityEngine;

namespace VRF.Utils
{
    /// <summary>
    /// Особенная утилита, которая притягивает объект к центру
    /// с использованием Rigidbody
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class RigidbodyIKReset : ValidatedMonoBehaviour
    {
        /*
        [SerializeField] private Vector3 scaleFactor = new(1, 0, 1);
        [Range(0, 1)] [SerializeField] private float velocityLerp = 0.8f;
        
#if UNITY_EDITOR
        [SerializeField, ReadOnly] private float scale;
        [SerializeField, ReadOnly] private Vector3 velocity; 
#endif
        */
        
        [Header("References")]
        [SerializeField] private Vector3 resetTarget;
        [SerializeField, HideInInspector] private Transform resetPos;
        [SerializeField, Self] private Transform tr;
        [SerializeField, Self] private Rigidbody rb;

        protected override void OnValidate()
        {
            base.OnValidate();
            rb.useGravity = false;
            rb.isKinematic = true;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
        /*
        private void Update()
        {
            // Дельта lerp функции (b-a)*t
            var scaleAverage = tr.lossyScale.Average();
            var t = scaleAverage * 0.8f;
            //rb.velocity = (resetPos.position - tr.position) * t;
            rb.velocity = (resetTarget - tr.localPosition) * t;
            rb.velocity = Vector3.Scale(rb.velocity, scaleFactor);
            
#if UNITY_EDITOR
            scale = scaleAverage;
            velocity = rb.velocity;
#endif
        }
        */
        public void ResetPos(bool x, bool y, bool z)
        {
            var pos = tr.localPosition;
            tr.localPosition = new Vector3(x ? pos.x : 0, y ? pos.y : 0, z ? pos.z : 0);
        }
    }
}