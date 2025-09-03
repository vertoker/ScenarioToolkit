using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using KBCore.Refs;
using UnityEngine;
using VRF.BNG_Framework.Scripts.Core;
using VRF.BNG_Framework.Scripts.Helpers;
using VRF.Utilities.Extensions;

namespace VRF.Players.Hands
{
    /// <summary>
    /// Контроллер скинов руки. Индивидуален для каждой руки, работает как стэк
    /// </summary>
    public class HandSkinsController : ValidatedMonoBehaviour
    {
        [SerializeField] private Transform skinsParent;
        [SerializeField] private Transform activeHandChild;
        [SerializeField] private bool searchSkinsOnStart = true;
        [Header("Content")]
        [SerializeField] private HandController handController;
        [SerializeField] private Grabber grabber;
        [SerializeField] private Grabber disableGrabber;
        [SerializeField] private RemoteGrabber remoteGrabber;
        [SerializeField] private RemoteGrabber disableRemoteGrabber;
        
        private readonly Stack<HandSkin> skins = new();
        private HandSkin activeSkin;

        public event Action<HandSkin> SkinEnabled; 
        public event Action<HandSkin> SkinDisabled; 

        public IEnumerable<HandSkin> Skins => skins;
        [CanBeNull] public HandSkin ActiveSkin => activeSkin;

        private void Start()
        {
            if (searchSkinsOnStart)
            {
                var spawnedSkins = skinsParent.GetComponentsInChildren<HandSkin>();
                foreach (var spawnedSkin in spawnedSkins)
                    PushSkin(spawnedSkin);
            }
        }

        // TODO сделать сетевую реализацию функции
        public void CreateSkin(HandSkin skinSource)
        {
            if (!skinSource)
            {
                Debug.LogWarning($"Empty source hand skin, drop", gameObject);
                return;
            }
            
            var handSkin = Instantiate(skinSource, skinsParent);
            PushSkin(handSkin);
        }
        public void PushSkin(HandSkin handSkin)
        {
            if (!handSkin)
            {
                Debug.LogWarning($"Empty hand skin, drop", gameObject);
                return;
            }
            
            DisableCurrentSkin();
            skins.Push(handSkin);
            EnableCurrentSkin();
        }
        public HandSkin PopSkin()
        {
            DisableCurrentSkin();
            var lastSkin = skins.Pop();
            EnableCurrentSkin();
            return lastSkin;
        }
        public void ClearInactive()
        {
            var lastSkin = skins.Pop();
            skins.Clear();
            skins.Push(lastSkin);
        }

        private void EnableCurrentSkin()
        {
            ResolveActiveSkin();
            if (!activeSkin) return;
            
            if (activeSkin.HandPhysics)
            {
                activeSkin.HandPhysics.ThisGrabber = grabber;
                activeSkin.HandPhysics.DisableGrabber = disableGrabber;
                activeSkin.HandPhysics.ThisRemoteGrabber = remoteGrabber;
                activeSkin.HandPhysics.DisableRemoteGrabber = disableRemoteGrabber;
            }
            if (handController)
            {
                handController.HandAnimator = activeSkin.HandAnimator;
                handController.handPoser = activeSkin.HandPoser;
                handController.autoPoser = activeSkin.AutoPoser;
            }
            if (activeHandChild)
            {
                activeHandChild.SetParent(activeSkin.transform, false);
                activeHandChild.ResetLocal();
            }

            SkinEnabled?.Invoke(activeSkin);
            activeSkin.HandObject.SetActive(true);
        }
        private void DisableCurrentSkin()
        {
            ResolveActiveSkin();
            if (!activeSkin) return;
            
            activeSkin.HandObject.SetActive(false);
            SkinDisabled?.Invoke(activeSkin);

            if (activeHandChild)
            {
                activeHandChild.SetParent(skinsParent, false);
                activeHandChild.ResetLocal();
            }
            if (handController)
            {
                handController.HandAnimator = null;
                handController.handPoser = null;
                handController.autoPoser = null;
            }
            if (activeSkin.HandPhysics)
            {
                activeSkin.HandPhysics.ThisGrabber = null;
                activeSkin.HandPhysics.DisableGrabber = null;
                activeSkin.HandPhysics.ThisRemoteGrabber = null;
                activeSkin.HandPhysics.DisableRemoteGrabber = null;
            }
        }

        private void ResolveActiveSkin()
        {
            activeSkin = skins.Count != 0 ? skins.Peek() : null;
        }
    }
}