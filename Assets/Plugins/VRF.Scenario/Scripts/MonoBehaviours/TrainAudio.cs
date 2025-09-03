using System.Collections.Generic;
using UnityEngine;

namespace VRF.Scenario.MonoBehaviours
{
    public class TrainAudio : MonoBehaviour
    {
        [SerializeField] private List<AudioClip> sounds;
        [SerializeField] private AudioSource audioSource1, audioSource2;
        [SerializeField] private AnimationCurve speedToIndex;
        [SerializeField] private float volume;
        
        public void SetSpeed(float speed)
        {
            var index = speedToIndex.Evaluate(speed);
            SetSounds(index);
        }

        private void SetSounds(float soundIndex)
        {
            var index1 = Mathf.FloorToInt(soundIndex);
            var index2 = index1 + 1;

            var frac = soundIndex - index1;
            var volume1 = 1 - frac;
            var volume2 = frac;

            if (index1 % 2 > 0)
            {
                (index1, index2) = (index2, index1);
                (volume1, volume2) = (volume2, volume1);
            }

            audioSource1.volume = volume1 * volume;
            audioSource2.volume = volume2 * volume;

            index1 = Mathf.Clamp(index1, 0, sounds.Count - 1);
            index2 = Mathf.Clamp(index2, 0, sounds.Count - 1);

            if (audioSource1.clip != sounds[index1])
            {
                audioSource1.clip = sounds[index1];
                audioSource1.Play();
            }

            if (audioSource2.clip != sounds[index2])
            {
                audioSource2.clip = sounds[index2];
                audioSource2.Play();
            }
        }
    }
}