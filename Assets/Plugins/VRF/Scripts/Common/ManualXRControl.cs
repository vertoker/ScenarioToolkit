using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Management;

namespace VRF.Utils
{
    [Obsolete]
    public class ManualXRControl
    {
        private readonly List<XRDisplaySubsystem> displaySubsystems = new List<XRDisplaySubsystem>();
        public bool IsVrActive { get; private set; }

        public IEnumerator StartXRCoroutine()
        {
            Debug.Log("Initializing XR...");
            yield return XRGeneralSettings.Instance.Manager.InitializeLoader();

            if (XRGeneralSettings.Instance.Manager.activeLoader == null)
            {
                Debug.LogError("Initializing XR Failed. Check Editor or Player log for details.");
                yield return null;
            }
            else
            {
                Debug.Log("Starting XR...");
                XRGeneralSettings.Instance.Manager.StartSubsystems();
                CheckVR();
            }
        }

        public async UniTask StartXRAsync()
        {
            await XRGeneralSettings.Instance.Manager.InitializeLoader();

            if (XRGeneralSettings.Instance.Manager.activeLoader == null)
            {
                Debug.LogError("Initializing XR Failed. Check Editor or Player log for details.");
                await UniTask.Yield();
            }
            else
            {
                Debug.Log("Starting XR...");
                XRGeneralSettings.Instance.Manager.StartSubsystems();
                CheckVR();
            }
        }
    
        public void StopXR()
        {
            Debug.Log("Stopping XR...");

            XRGeneralSettings.Instance.Manager.StopSubsystems();
            XRGeneralSettings.Instance.Manager.DeinitializeLoader();
            Debug.Log("XR stopped completely.");
        }
    
        private void CheckVR()
        {
            SubsystemManager.GetSubsystems<XRDisplaySubsystem>(displaySubsystems);
            Debug.Log("VRStatusChecker start");
            if (displaySubsystems.Count > 0)
                IsVrActive = true;
            foreach (var xrDisplaySubsystem in displaySubsystems)
            {
                Debug.Log(xrDisplaySubsystem);
            }
            Debug.Log("VRStatusChecker end");
        }
    }
}

