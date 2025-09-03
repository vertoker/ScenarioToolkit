using NaughtyAttributes;
using UnityEngine;

namespace VRF.Utils
{
    /// <summary>
    /// Utility middle-class для более удобной работы с editor
    /// </summary>
    public class ActivatedMonoBehaviour : MonoBehaviour
    {
#if UNITY_EDITOR
        [ShowNativeProperty] public bool IsEnabled => enabled;
        [Button] private void Enable() { enabled = true; }
        [Button] private void Disable() { enabled = false; }
#endif
    }
}