using UnityEngine;
using VRF.DataSources.Scriptables.Base;

namespace VRF.Players.Models.Player
{
    /// <summary>
    /// Реализация AuthIdentityModel для Scriptables
    /// </summary>
    [CreateAssetMenu(fileName = nameof(PlayerConfig), menuName = "VRF/DataSources/" + nameof(PlayerConfig))]
    public class PlayerConfig : BaseScriptableModel<PlayerModel>
    {
        
    }
}