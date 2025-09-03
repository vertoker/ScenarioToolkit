using System.Collections.Generic;
using System.Linq;
using KBCore.Refs;
using Mirror;
using NaughtyAttributes;
using UnityEngine;
using VRF.BNG_Framework.HandPoser.Scripts;
using VRF.Utilities;

namespace VRF.Players.Hands
{
    /// <summary> Надстройка для удобной синхронизации пальцев между собой </summary>
    [RequireComponent(typeof(HandPoser))]
    public class HandPoserUpdater : ValidatedMonoBehaviour
    {
        [SerializeField, Self] private HandPoser poser;

        public IReadOnlyList<Transform> Thumbs => poser.ThumbJoints;
        public IReadOnlyList<Transform> Indexes => poser.IndexJoints;
        public IReadOnlyList<Transform> Middles => poser.MiddleJoints;
        public IReadOnlyList<Transform> Rings => poser.RingJoints;
        public IReadOnlyList<Transform> Pinkies => poser.PinkyJoints;
        
        public Transform Wrist => poser.WristJoint;
        public IReadOnlyList<Transform> Others => poser.OtherJoints;
        
        public void SyncFrom(HandPoserUpdater observer, bool syncPosition = false, bool syncRotation = true, bool syncScale = false)
        {
            SyncJoints(Wrist, observer.Wrist, syncPosition, syncRotation, syncScale);
            SyncJoints(Thumbs, observer.Thumbs, syncPosition, syncRotation, syncScale);
            SyncJoints(Indexes, observer.Indexes, syncPosition, syncRotation, syncScale);
            SyncJoints(Middles, observer.Middles, syncPosition, syncRotation, syncScale);
            SyncJoints(Rings, observer.Rings, syncPosition, syncRotation, syncScale);
            SyncJoints(Pinkies, observer.Pinkies, syncPosition, syncRotation, syncScale);
            SyncJoints(Others, observer.Others, syncPosition, syncRotation, syncScale);
        }

        public void SyncFrom(IReadOnlyList<Transform> thumbBones, 
            IReadOnlyList<Transform> indexBones,
            IReadOnlyList<Transform> middleBones,
            IReadOnlyList<Transform> ringBones,
            IReadOnlyList<Transform> pinkyBones, 
            bool syncPosition = false, bool syncRotation = true, bool syncScale = false)
        {
            SyncJoints(Thumbs, thumbBones, syncPosition, syncRotation, syncScale);
            SyncJoints(Indexes, indexBones, syncPosition, syncRotation, syncScale);
            SyncJoints(Middles, middleBones, syncPosition, syncRotation, syncScale);
            SyncJoints(Rings, ringBones, syncPosition, syncRotation, syncScale);
            SyncJoints(Pinkies, pinkyBones, syncPosition, syncRotation, syncScale);
        }

        public void SyncTo(IReadOnlyList<Transform> thumbBones, 
            IReadOnlyList<Transform> indexBones,
            IReadOnlyList<Transform> middleBones,
            IReadOnlyList<Transform> ringBones,
            IReadOnlyList<Transform> pinkyBones, 
            bool syncPosition = false, bool syncRotation = true, bool syncScale = false)
        {
            SyncJoints(thumbBones, Thumbs, syncPosition, syncRotation, syncScale);
            SyncJoints(indexBones, Indexes, syncPosition, syncRotation, syncScale);
            SyncJoints(middleBones, Middles, syncPosition, syncRotation, syncScale);
            SyncJoints(ringBones, Rings, syncPosition, syncRotation, syncScale);
            SyncJoints(pinkyBones, Pinkies, syncPosition, syncRotation, syncScale);
        }

        private void SyncJoints(IReadOnlyList<Transform> receivers, IReadOnlyList<Transform> observers, 
            bool syncPosition, bool syncRotation, bool syncScale)
        {
            var length = receivers.Count;
            var length2 = observers.Count;

            if (length != length2)
            {
                //Debug.LogWarning("Lengths of both is doesn't equals, sync by min length", gameObject);
                length = Mathf.Min(length, length2);
            }

            for (var i = 0; i < length; i++)
            {
                SyncJoints(receivers[i], observers[i], syncPosition, syncRotation, syncScale);
            }
        }

        private void SyncJointsInverse(IReadOnlyList<Transform> receivers, IReadOnlyList<Transform> observers,
            bool syncPosition, bool syncRotation, bool syncScale)
        {
            var length = receivers.Count;
            var length2 = observers.Count;

            if (length != length2)
            {
                //Debug.LogWarning("Lengths of both is doesn't equals, sync by min length", gameObject);
                length = Mathf.Min(length, length2);
            }

            for (var i = 0; i < length; i++)
            {
                SyncJoints(receivers[i], observers[i], syncPosition, syncRotation, syncScale);
            }
        }

        private void SyncJoints(Transform receiver, Transform observer, bool syncPosition, bool syncRotation, bool syncScale)
        {
            if (receiver == null)
            {
                Debug.LogError("Null receiver, drop that sync", gameObject);
                return;
            }
            
            if (observer == null)
            {
                Debug.LogError("Null observer, drop that sync", gameObject);
                return;
            }
                
            if (syncPosition) receiver.position = observer.position;
            if (syncRotation) receiver.rotation = observer.rotation;
            if (syncScale) receiver.localScale = observer.localScale;
        }

        [Button]
        public void EnsureNetworkTransforms()
        {
            poser.EnsureNetworkTransforms();
            Debug.Log(nameof(EnsureNetworkTransforms));
            
            UpdateSyncFields();
        }
        private void UpdateSyncFields()
        {
            var list = poser.GetTransformsList();
            foreach (var netComponent 
                     in list.Select(joint => joint.GetComponent<NetworkTransformUnreliable>()))
            {
                netComponent.syncPosition = false;
                netComponent.syncRotation = true;
                netComponent.syncScale = false;
            }
        }

        [Button]
        public void RemoveNetworkTransforms()
        {
            poser.RemoveNetworkTransforms();
            Debug.Log(nameof(RemoveNetworkTransforms));
        }
    }
}