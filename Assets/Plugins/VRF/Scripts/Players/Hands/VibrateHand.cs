using NaughtyAttributes;
using UnityEngine;
using VRF.BNG_Framework.Scripts.Core;

namespace VRF.Players.Hands
{
    public class VibrateHand : MonoBehaviour
    {
        [SerializeField] private VibrateConfig vibrateConfig;
        [SerializeField] private ControllerHand hand = ControllerHand.Right;

        public VibrateConfig VibrateConfig
        {
            get => vibrateConfig;
            set => vibrateConfig = value;
        }
        public ControllerHand Hand
        {
            get => hand;
            set => hand = value;
        }

        [Button]
        public void Vibrate()
        {
            if (!vibrateConfig) return;
            Vibrate(vibrateConfig);
        }
        public void Vibrate(VibrateConfig config)
        {
            if (!IsPlaying()) return;
            InputBridge.Instance.VibrateController(config.Frequency, config.Amplitude, config.Duration, hand);
        }
        
        private static bool IsPlaying()
        {
#if UNITY_EDITOR
            if (UnityEditor.EditorApplication.isPlaying) return true;
#endif
            return Application.isPlaying;
        }
    }
}