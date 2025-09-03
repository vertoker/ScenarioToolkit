using System;
using UnityEngine;
using VRF.Players.Hands;

namespace VRF.Players.Checking
{
    [Serializable]
    public class CheckingModel
    {
        [SerializeField] private bool useOnlyOne = true;
        [SerializeField] private bool logActions = false;
        [Header("Vibrate")]
        [SerializeField] private bool vibrateOnStart = false;
        [SerializeField] private VibrateConfig vibrateStart = null;
        [SerializeField] private bool vibrateOnSuccess = true;
        [SerializeField] private VibrateConfig vibrateSuccess = null;
        [SerializeField] private bool vibrateOnPressedFailed = false;
        [SerializeField] private VibrateConfig vibratePressedFailed = null;
        [SerializeField] private bool vibrateOnHoveredFailed = true;
        [SerializeField] private VibrateConfig vibrateHoveredFailed = null;

        public bool UseOnlyOne => useOnlyOne;
        public bool LogActions => logActions;
        
        public bool VibrateOnStart => vibrateOnStart;
        public VibrateConfig VibrateStart => vibrateStart;
        public bool VibrateOnSuccess => vibrateOnSuccess;
        public VibrateConfig VibrateSuccess => vibrateSuccess;

        public bool VibrateOnPressedFailed => vibrateOnPressedFailed;
        public VibrateConfig VibratePressedFailed => vibratePressedFailed;
        public bool VibrateOnHoveredFailed => vibrateOnHoveredFailed;
        public VibrateConfig VibrateHoveredFailed => vibrateHoveredFailed;
    }
}