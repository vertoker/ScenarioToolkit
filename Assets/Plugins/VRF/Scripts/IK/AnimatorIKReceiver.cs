using System;
using KBCore.Refs;
using UnityEngine;

namespace VRF.IK
{
    /// <summary>
    /// Раздатчик ивента OnAnimatorIK для всех подписчиков,
    /// класс существует из-за того, что объект аниматор пробрасывает
    /// этот метод только в GameObject, где находится сам
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class AnimatorIKReceiver : ValidatedMonoBehaviour
    {
        [SerializeField, Self] private Animator animator;

        public Animator Animator => animator;

        public event Action<int> OnAnimatorUpdateIK;

        private void OnAnimatorIK(int layerIndex)
        {
            OnAnimatorUpdateIK?.Invoke(layerIndex);
        }
    }
}