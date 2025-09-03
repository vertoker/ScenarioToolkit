using UnityEngine;
using VRF.Players.Controllers.Models;

namespace VRF.Players.Controllers.Scriptables
{
    [CreateAssetMenu(fileName = nameof(PlayerVRConfig), menuName = "VRF/Player/" + nameof(PlayerVRConfig))]
    public class PlayerVRConfig : BaseLocalPlayerConfig
    {
        [field:SerializeField] public HandViewModel HandView { get; set; } = new();
    }
}