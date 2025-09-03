using UnityEngine;
using VRF.DataSources.Scriptables.Base;

namespace VRF.Networking.Models
{
    /// <summary>
    /// Реализация NetModel для Scriptables
    /// </summary>
    [CreateAssetMenu(fileName = nameof(NetConfig), menuName = "VRF/DataSources/" + nameof(NetConfig))]
    public class NetConfig : BaseScriptableModel<NetModel>
    {
        
    }
}