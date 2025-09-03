using UnityEngine;
using VRF.DataSources.Scriptables.Base;

namespace VRF.Identities.Models
{
    /// <summary>
    /// Реализация AuthIdentityModel для Scriptables
    /// </summary>
    [CreateAssetMenu(fileName = nameof(AuthIdentityConfig), menuName = "VRF/DataSources/" + nameof(AuthIdentityConfig))]
    public class AuthIdentityConfig : BaseScriptableModel<AuthIdentityModel>
    {
        
    }
}