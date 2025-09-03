using UnityEngine;
using UnityEngine.EventSystems;

namespace VRF.Players.Scriptables
{
    [CreateAssetMenu(fileName = nameof(ProjectPlayerSpawnConfig), menuName = "VRF/Player/" + nameof(ProjectPlayerSpawnConfig))]
    public class ProjectPlayerSpawnConfig : ScriptableObject
    {
        [field:Header("VR")]
        [field:SerializeField] public EventSystem EventSystemVR { get; set; }
        
        [field:Header("WASD")]
        [field:SerializeField] public EventSystem EventSystemWASD { get; set; }
    }
}