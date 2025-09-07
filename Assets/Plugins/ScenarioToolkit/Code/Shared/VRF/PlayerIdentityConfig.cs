using NaughtyAttributes;
using ScenarioToolkit.Shared.VRF.Utilities.Extensions;
using UnityEngine;

namespace ScenarioToolkit.Shared.VRF
{
    /// <summary>
    /// Главный конфиг для идентификации игрока, используется даже если Identities система не инициализирована
    /// </summary>
    [CreateAssetMenu(fileName = nameof(PlayerIdentityConfig), menuName = "VRF/Identities/" + nameof(PlayerIdentityConfig))]
    public class PlayerIdentityConfig : IdentifiedScriptableObject
    {
        [SerializeField] private string roleName;
        [SerializeField] private bool hasLimit;

        [ShowIf(nameof(hasLimit))]
        [SerializeField] private int limitCount = 1;

        public string RoleName => roleName;
        
        public bool HasLimit => hasLimit;
        public int LimitCount => limitCount;

        [Space]
        [SerializeField] private AuthIdentityModel authIdentityModel = new(string.Empty);
        public AuthIdentityModel AuthIdentityModel => authIdentityModel;

        [Space] [Expandable]
        [SerializeField] private PlayerAppearanceConfig appearance;
        public PlayerAppearanceConfig Appearance => appearance;

        private bool HasAppearance => appearance != null;

        [HideIf(nameof(HasAppearance))]
        [Button]
        private void CreateAppearance()
        {
            appearance = VrfHashCodeExtensions.CreateOrLoadPreset<PlayerAppearanceConfig>(this.GetPath());
        }
    }
}