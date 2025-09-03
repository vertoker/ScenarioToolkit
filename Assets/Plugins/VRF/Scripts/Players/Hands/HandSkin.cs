using System;
using JetBrains.Annotations;
using NaughtyAttributes;
using UnityEngine;
using VRF.BNG_Framework.HandPoser.Scripts;
using VRF.BNG_Framework.Scripts.Helpers;
using VRF.Networking.Models;

namespace VRF.Players.Hands
{
    /// <summary>
    /// Репрезентация скина руки. Главное условие - отсутствие HandController.
    /// Опционально может иметь физику HandPhysics
    /// </summary>
    public class HandSkin : MonoBehaviour
    {
        [SerializeField, ReadOnly] private GameObject handObject;
        [SerializeField] private Transform ikPoint;
        [SerializeField, ReadOnly] private Animator handAnimator;
        [SerializeField, ReadOnly] private HandPoser handPoser;
        [SerializeField, ReadOnly] private HandPoserUpdater handPoserUpdater;
        [SerializeField, ReadOnly] private AutoPoser autoPoser;
        [SerializeField, ReadOnly] private HandPhysics handPhysics;

        public GameObject HandObject => handObject;
        public Transform IKPoint => ikPoint;
        public Animator HandAnimator => handAnimator;
        public HandPoser HandPoser => handPoser;
        public HandPoserUpdater HandPoserUpdater => handPoserUpdater;
        public AutoPoser AutoPoser => autoPoser;
        [CanBeNull] public HandPhysics HandPhysics => handPhysics;

        private void OnValidate()
        {
            handObject = gameObject;
            if (!ikPoint) ikPoint = transform;
            handAnimator = GetComponentInChildren<Animator>();
            handPoser = GetComponentInChildren<HandPoser>();
            handPoserUpdater = GetComponentInChildren<HandPoserUpdater>();
            autoPoser = GetComponentInChildren<AutoPoser>();
            handPhysics = GetComponentInChildren<HandPhysics>();
        }
        
        public void SetNet()
        {
            // Для Observer не надо, так как он изначально Observer
            handAnimator.enabled = false;
            autoPoser.enabled = false;
            //handPoserUpdater.EnsureNetworkTransforms();
        }

        // Костыль из-за того, что физическая рука по какой-то причине вылетает из игрока и становиться независимой
        // И при отключении объекта в SkinsController, HandPhysics ломается
        private void OnEnable()
        {
            if (handPhysics)
                handPhysics.gameObject.SetActive(true);
        }
        private void OnDisable()
        {
            if (handPhysics)
                handPhysics.gameObject.SetActive(false);
        }
    }
}