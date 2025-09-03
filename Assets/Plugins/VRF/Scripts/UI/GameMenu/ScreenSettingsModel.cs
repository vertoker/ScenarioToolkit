using System;
using NaughtyAttributes;
using SimpleUI.Anim;
using SimpleUI.Extensions;
using SimpleUI.Installers.ModelSettings;
using UnityEngine;
using VRF.Utilities.Attributes;

namespace VRF.UI.GameMenu
{
    [Serializable]
    public class ScreenSettingsModel : BaseScreenSettingsModel
    {
        [Header("Anim")]
        [SerializeField] private ScreenAnim animType = ScreenAnim.Scale;
        [SerializeField] private float animTime = 0.4f;
        [SerializeField] private Easings.Type easingType = Easings.Type.Linear;
        [Header("Show")]
        [SerializeField] private bool showInventory = true;
        [SerializeField] private bool showDialog = false;
        [SerializeField] private bool showControls = true;
        [SerializeField] private bool showAbout = false;
        [SerializeField] private bool showSettings = true;
        [SerializeField] private bool showQuitApplication = true;
        [SerializeField] private bool showQuitToScene = false;
        [SceneReference] [ShowIf(nameof(showQuitToScene))] [AllowNesting]
        [SerializeField] private string toQuitScene = string.Empty;
        [Header("Sounds")]
        [SerializeField] private bool useOnEnable = true;
        [SerializeField] private AudioClip enableSound;
        [SerializeField] private bool useOnDisable = false;
        [SerializeField] private AudioClip disableSound;
        
        public ScreenAnim AnimType => animType;
        public float AnimTime => animTime;
        public Easings.Type EasingType => easingType;

        public bool ShowInventory => showInventory;
        public bool ShowDialog => showDialog;
        public bool ShowControls => showControls;
        public bool ShowAbout => showAbout;
        public bool ShowSettings => showSettings;
        public bool ShowQuitApplication => showQuitApplication;
        public bool ShowQuitToScene => showQuitToScene;
        public string ToQuitScene => toQuitScene;
        
        public bool UseOnEnable => useOnEnable;
        public AudioClip EnableSound => enableSound;
        public bool UseOnDisable => useOnDisable;
        public AudioClip DisableSound => disableSound;
    }
}