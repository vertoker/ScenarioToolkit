using System;
using SimpleUI.Anim;
using SimpleUI.Extensions;
using SimpleUI.Installers.ModelSettings;
using UnityEngine;

namespace VRF.Scenario.UI.Game.InfoTip
{
    public class ScreenSettingsInstaller : BaseScreenSettingsInstaller<ScreenSettingsModel> { }
    
    [Serializable]
    public class ScreenSettingsModel : BaseScreenSettingsModel
    {
        [Header("Anim")]
        [SerializeField] private ScreenAnim animType = ScreenAnim.Scale;
        [SerializeField] private float animTime = 0.4f;
        [SerializeField] private Easings.Type easingType = Easings.Type.Linear;
        [Header("Sounds")]
        [SerializeField] private bool useNotification = true;
        [SerializeField] private AudioClip notificationSound;
        [SerializeField] private bool useOnEnable = true;
        [SerializeField] private AudioClip enableSound;
        [SerializeField] private bool useOnDisable = false;
        [SerializeField] private AudioClip disableSound;
        
        public ScreenAnim AnimType => animType;
        public float AnimTime => animTime;
        public Easings.Type EasingType => easingType;

        public bool UseNotification => useNotification;
        public AudioClip NotificationSound => notificationSound;
        public bool UseOnEnable => useOnEnable;
        public AudioClip EnableSound => enableSound;
        public bool UseOnDisable => useOnDisable;
        public AudioClip DisableSound => disableSound;
    }
}