using System;
using UnityEngine;
using UObject = UnityEngine.Object;
using SObject = System.Object;

namespace VRF.Utilities
{
    /// <summary>
    /// Различные обёртки над часто используемыми типами (чтобы вместо typeof(T) можно было писать VrfTypes.T)
    /// </summary>
    public static class VrfTypes
    {
        public static readonly Type SystemObject = typeof(SObject);
        public static readonly Type Type = typeof(Type);
        public static readonly Type Attribute = typeof(Attribute);
        
        public static readonly Type UnityObject = typeof(UObject);
        public static readonly Type Component = typeof(Component);
        public static readonly Type Behaviour = typeof(Behaviour);
        public static readonly Type MonoBehaviour = typeof(MonoBehaviour);
        
        public static readonly Type GameObject = typeof(GameObject);
        public static readonly Type Transform = typeof(Transform);
    }
}