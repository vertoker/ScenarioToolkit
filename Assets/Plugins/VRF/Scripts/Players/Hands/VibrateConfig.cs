using UnityEngine;

namespace VRF.Players.Hands
{
    [CreateAssetMenu(fileName = nameof(VibrateConfig), menuName = "VRF/Players/" + nameof(VibrateConfig))]
    public class VibrateConfig : ScriptableObject
    {
        [Range(0, 1)] [SerializeField] private float frequency = 0.2f;
        [Range(0, 1)] [SerializeField] private float amplitude = 0.2f;
        [Range(0, 1)] [SerializeField] private float duration = 0.2f;

        public float Frequency => frequency;
        public float Amplitude => amplitude;
        public float Duration => duration;
    }
}