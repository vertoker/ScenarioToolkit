using NaughtyAttributes;
using UnityEngine;
using VRF.BNG_Framework.Scripts.Core;

namespace VRF.Test
{
    public class TestVibrateController : MonoBehaviour
    {
        [Range(0, 1)] public float frequency = 0.2f;
        [Range(0, 1)] public float amplitude = 0.2f;
        [Range(0, 1)] public float duration = 0.2f;
        
        [Button(enabledMode: EButtonEnableMode.Playmode)]
        private void VibrateLeft()
        {
            InputBridge.Instance.VibrateController(frequency, amplitude, duration, ControllerHand.Left);
        }
        [Button(enabledMode: EButtonEnableMode.Playmode)]
        private void VibrateRight()
        {
            InputBridge.Instance.VibrateController(frequency, amplitude, duration, ControllerHand.Right);
        }
    }
}