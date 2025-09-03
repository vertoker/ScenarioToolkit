using UnityEngine;
using UnityEngine.XR.Management;
using Zenject;

namespace VRF.Players.Installers
{
    public class XRLoaderInstaller : MonoInstaller
    {
        public void Awake()
        {
            if (XRGeneralSettings.Instance 
                && XRGeneralSettings.Instance.Manager 
                && !XRGeneralSettings.Instance.Manager.isInitializationComplete)
            {
                // TODO когда нибудь починить
                //XRGeneralSettings.Instance.Manager.InitializeLoaderSync();
                //XRGeneralSettings.Instance.Manager.StartSubsystems();
            }
        }

        public override void InstallBindings()
        {
            
        }
    }
}