using UnityEngine;
using UnityEngine.Serialization;
using VRF.Players.Raycasting;

// ReSharper disable once CheckNamespace
namespace VRF.VRBehaviours.Checking
{
    public class Checkable : BaseRaycastable
    {
        [FormerlySerializedAs("_checkingTime")] // Не трогать
        [SerializeField] private float checkingTime = 1f;

        public float CheckingTime
        {
            get => checkingTime;
            set => checkingTime = value;
        }
    }
}