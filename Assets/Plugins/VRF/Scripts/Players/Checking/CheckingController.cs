using System;
using System.Collections;
using UnityEngine;
using VRF.Players.Hands;
using VRF.Players.Raycasting;
using VRF.VRBehaviours.Checking;
using Zenject;

namespace VRF.Players.Checking
{
    public class CheckingController : MonoBehaviour
    {
        [SerializeField] private PhysicsRaycast raycaster;
        [SerializeField] private VibrateHand vibrate;

        private Coroutine loading;
        private Checkable currentCheckable;
        
        [InjectOptional]
        private CheckingModel checkingModel;

        public event Action<Checkable> CheckStarted;
        public event Action<float> CheckProgress;
        public event Action<Checkable> CheckStopped;
        public event Action<Checkable> CheckSuccessful;
        public event Action<Checkable> CheckFailed;

        private void OnEnable()
        {
            raycaster.ButtonPress += TryStartCheck;
            raycaster.HoverStart += TryStartCheck;
            raycaster.ButtonRelease += TryFailPressedCheck;
            raycaster.HoverEnd += TryFailHoveredCheck;
        }
        private void OnDisable()
        {
            raycaster.ButtonPress -= TryStartCheck;
            raycaster.HoverStart -= TryStartCheck;
            raycaster.ButtonRelease -= TryFailPressedCheck;
            raycaster.HoverEnd -= TryFailHoveredCheck;
        }

        private void TryStartCheck(Transform tr)
        {
            if (raycaster.IsHoveredAndPressed)
                StartCheck(tr);
        }
        private void TryFailPressedCheck(Transform tr)
        {
            if (loading != null)
                FailCheck(false);
        }
        private void TryFailHoveredCheck(Transform tr)
        {
            if (loading != null)
                FailCheck(true);
        }
        
        public void StartCheck(Transform t)
        {
            if (!t.TryGetComponent<Checkable>(out var checkable)) return;
            if (!checkable.isActiveAndEnabled) return;
            
            StopCoroutine();
            currentCheckable = checkable;
            
            if (vibrate && checkingModel is { VibrateOnStart: true } && checkingModel.VibrateStart)
                vibrate.Vibrate(checkingModel.VibrateStart);
            
            if (checkingModel is { LogActions: true })
                Debug.Log($"Check started");
            CheckStarted?.Invoke(checkable);
            
            loading = StartCoroutine(Loading());
        }

        public void SuccessCheck()
        {
            StopCheck();
            
            if (vibrate && checkingModel is { VibrateOnSuccess: true } && checkingModel.VibrateSuccess)
                vibrate.Vibrate(checkingModel.VibrateSuccess);
            
            if (checkingModel is { LogActions: true })
                Debug.Log($"Check successful");
            CheckSuccessful?.Invoke(currentCheckable);
        }
        public void FailCheck(bool hovered)
        {
            StopCheck();

            if (hovered)
            {
                if (checkingModel is { VibrateOnHoveredFailed: true } && checkingModel.VibrateHoveredFailed)
                    vibrate?.Vibrate(checkingModel.VibrateHoveredFailed);
            }
            else
            {
                if (checkingModel is { VibrateOnPressedFailed: true } && checkingModel.VibratePressedFailed)
                    vibrate?.Vibrate(checkingModel.VibratePressedFailed);
            }
            
            if (checkingModel is { LogActions: true })
                Debug.Log($"Check failed");
            CheckFailed?.Invoke(currentCheckable);
        }
        public void StopCheck()
        {
            StopCoroutine();
            
            if (checkingModel is { LogActions: true })
                Debug.Log($"Check stopped");
            CheckStopped?.Invoke(currentCheckable);
        }
        public void StopCoroutine()
        {
            if (loading != null)
            {
                StopCoroutine(loading);
                loading = null;
            }
        }
        
        private IEnumerator Loading()
        {
            for (var i = 0f; i <= 1f; i += Time.deltaTime / currentCheckable.CheckingTime)
            {
                if (checkingModel is { LogActions: true })
                    Debug.Log($"{gameObject.name}: {i * 100}%");
                CheckProgress?.Invoke(i);
                yield return null;
            }
            CheckProgress?.Invoke(1f);
            
            SuccessCheck();
        }
    }
}
