using System;
using System.Linq;
using Mirror;
using NaughtyAttributes;
using UnityEngine;

namespace VRF.Editor
{
    [CreateAssetMenu(fileName  = nameof(MirrorHashCodeResearcher), menuName = "VRF/Testing/" + nameof(MirrorHashCodeResearcher))]
    public class MirrorHashCodeResearcher : ScriptableObject
    {
        [SerializeField] private ushort toFindCode = 57574;
        
        [Button]
        private void ResearchAllTypes()
        {
            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes());

            foreach (var type in types)
            {
                var id = type.FullName.GetStableHashCode16();
                if (id == toFindCode)
                {
                    Debug.Log($"Match at {type.FullName} ({id})");
                }
            }
        }
    }
}