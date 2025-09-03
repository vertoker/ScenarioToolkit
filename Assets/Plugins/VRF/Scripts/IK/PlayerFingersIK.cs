using System.Collections.Generic;
using KBCore.Refs;
using UnityEngine;
using VRF.Players.Hands;

namespace VRF.IK
{
    /// <summary>
    /// Синхронизация пальцев для игрока (пока не работает)
    /// </summary>
    public class PlayerFingersIK : ValidatedMonoBehaviour
    {
        [Header("Core")]
        [SerializeField] private bool ikFingersActive = true;

        [Header("References")]
        [SerializeField] private HandPoserUpdater leftHand;
        [SerializeField] private HandPoserUpdater rightHand;
        [SerializeField] private AnimatorIKReceiver animatorIK;

        private static readonly HumanBodyBones[] LThumbs = 
            { HumanBodyBones.LeftThumbProximal, HumanBodyBones.LeftThumbIntermediate, HumanBodyBones.LeftThumbDistal };
        private static readonly HumanBodyBones[] LIndexes = 
            { HumanBodyBones.LeftIndexProximal, HumanBodyBones.LeftIndexIntermediate, HumanBodyBones.LeftIndexDistal };
        private static readonly HumanBodyBones[] LMiddles = 
            { HumanBodyBones.LeftMiddleProximal, HumanBodyBones.LeftMiddleIntermediate, HumanBodyBones.LeftMiddleDistal };
        private static readonly HumanBodyBones[] LRings = 
            { HumanBodyBones.LeftRingProximal, HumanBodyBones.LeftRingIntermediate, HumanBodyBones.LeftRingDistal };
        private static readonly HumanBodyBones[] LPinkies = 
            { HumanBodyBones.LeftLittleProximal, HumanBodyBones.LeftLittleIntermediate, HumanBodyBones.LeftLittleDistal };

        private static readonly HumanBodyBones[] RThumbs = 
            { HumanBodyBones.RightThumbProximal, HumanBodyBones.RightThumbIntermediate, HumanBodyBones.RightThumbDistal };
        private static readonly HumanBodyBones[] RIndexes = 
            { HumanBodyBones.RightIndexProximal, HumanBodyBones.RightIndexIntermediate, HumanBodyBones.RightIndexDistal };
        private static readonly HumanBodyBones[] RMiddles = 
            { HumanBodyBones.RightMiddleProximal, HumanBodyBones.RightMiddleIntermediate, HumanBodyBones.RightMiddleDistal };
        private static readonly HumanBodyBones[] RRings = 
            { HumanBodyBones.RightRingProximal, HumanBodyBones.RightRingIntermediate, HumanBodyBones.RightRingDistal };
        private static readonly HumanBodyBones[] RPinkies = 
            { HumanBodyBones.RightLittleProximal, HumanBodyBones.RightLittleIntermediate, HumanBodyBones.RightLittleDistal };

        private void OnEnable() { if (animatorIK) animatorIK.OnAnimatorUpdateIK += OnAnimatorIK; }
        private void OnDisable() { if (animatorIK) animatorIK.OnAnimatorUpdateIK -= OnAnimatorIK; }

        private void OnAnimatorIK(int layerIndex)
        {
            if (ikFingersActive)
            {
                SyncBones(LThumbs, leftHand.Thumbs);
                SyncBones(LIndexes, leftHand.Indexes);
                SyncBones(LMiddles, leftHand.Middles);
                SyncBones(LRings, leftHand.Rings);
                SyncBones(LPinkies, leftHand.Pinkies);
                
                SyncBones(RThumbs, rightHand.Thumbs);
                SyncBones(RIndexes, rightHand.Indexes);
                SyncBones(RMiddles, rightHand.Middles);
                SyncBones(RRings, rightHand.Rings);
                SyncBones(RPinkies, rightHand.Pinkies);
            }
        }

        private void SyncBones(IReadOnlyList<HumanBodyBones> bones, IReadOnlyList<Transform> sources)
        {
            // TODO починить синхронизацию поворота пальцев
            // До тех пор этот скрипт стоит держать отключенным принудительно
            
            /*
            var length = Mathf.Min(bones.Count, sources.Count);

            for (var i = 0; i < length; i++)
                animator.SetBoneLocalRotation(bones[i], sources[i].localRotation);
            */
        }
    }
}