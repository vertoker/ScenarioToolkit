using System.Collections.Generic;
using JetBrains.Annotations;
using Zenject;

namespace ScenarioToolkit.Shared.VRF
{
    /// <summary>
    /// Сервис идентификации, прокидывает все нужные данные
    /// для идентификации игроков и их обликов.
    /// Работает на всех игроках, данные берёт локально
    /// </summary>
    public class IdentityService
    {
        private readonly IdentitiesConfig list;
        
        private readonly Dictionary<int, PlayerIdentityConfig> identities;
        private readonly Dictionary<int, PlayerAppearanceConfig> appearances;
        
        /// <summary> Идентификация локального игрока </summary>
        public PlayerIdentityConfig SelfIdentity { get; private set; }
        /// <summary> Список всех аккаунтов (локальный у каждого) </summary>
        public IReadOnlyDictionary<int, PlayerIdentityConfig> Identities => identities;
        /// <summary> Список всех отображений (локальный у каждого) </summary>
        public IReadOnlyDictionary<int, PlayerAppearanceConfig> Appearances => appearances;

        public IdentityService([InjectOptional, CanBeNull] IdentitiesConfig list)
        {
            this.list = list;
            
            identities = list ? list.IdentitiesToDictionary() : new Dictionary<int, PlayerIdentityConfig>();
            appearances = list ? list.AppearancesToDictionary() : new Dictionary<int, PlayerAppearanceConfig>();
        }
        
        public bool TryInitializeSelfIdentity(AuthIdentityModel authModel)
        {
            if (SelfIdentity) return false;
            if (!list) return false;
            SelfIdentity = list.FindIdentity(authModel);
            return true;
        }
        public bool ClearSelfIdentity()
        {
            if (!SelfIdentity) return false;
            SelfIdentity = null;
            return true;
        }
    }
}