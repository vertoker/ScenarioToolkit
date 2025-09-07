using System.Collections.Generic;
using System.Linq;
using Mirror;
using NaughtyAttributes;
using UnityEngine;

namespace ScenarioToolkit.Shared.VRF
{
    /// <summary>
    /// Все данные ролей игроков, сильно связан с сетью. У всех сетевых игроков есть его локальная копия
    /// </summary>
    [CreateAssetMenu(fileName = nameof(IdentitiesConfig), menuName = "VRF/Identities/" + nameof(IdentitiesConfig))]
    public class IdentitiesConfig : ScriptableObject
    {
        [Header("Identities"), Expandable]
        [SerializeField] private PlayerIdentityConfig[] configs;
        
        // Если игрок не может авторизоваться, то ему отправляется анонимная роль, иначе просто отключает
        [SerializeField] private bool allowAnonymousIdentities = true;
        
        [ShowIf(nameof(allowAnonymousIdentities)), Expandable, Required]
        [SerializeField] private PlayerIdentityConfig defaultConfig;
        
        [Header("Register"), Expandable]
        [SerializeField] private PlayerAppearanceConfig[] appearances;

        #region Identities
        public PlayerIdentityConfig FindIdentity(AuthIdentityModel model)
        {
            var identity = configs.FirstOrDefault(c => c.AuthIdentityModel.Compare(model));
            if (identity) return identity;

            if (allowAnonymousIdentities)
            {
                if (defaultConfig.AuthIdentityModel.Compare(model))
                    return defaultConfig;
            }

            Debug.LogError("Can't find any identity, use null, but it must not be like this");
            return null;
        }
        public bool TryFindIdentity<TAuthMessage>(TAuthMessage msg, out PlayerIdentityConfig identity) 
            where TAuthMessage : struct, NetworkMessage
        {
            var result = configs
                .FirstOrDefault(config => config.AuthIdentityModel.Compare(msg));

            if (result != null)
            {
                identity = result;
                return true;
            }
            
            if (allowAnonymousIdentities)
            {
                identity = defaultConfig;
                return true;
            }

            identity = null;
            return false;
        }
        #endregion
        
        #region Collections
        public IEnumerable<PlayerIdentityConfig> IdentitiesToEnumerable()
        {
            if (!allowAnonymousIdentities) return configs;

            var list = new List<PlayerIdentityConfig>(configs.Length + 1);
            list.AddRange(configs);
            list.Add(defaultConfig);
            return list;
        }
        public Dictionary<int, PlayerIdentityConfig> IdentitiesToDictionary()
        {
            var dict = new Dictionary<int, PlayerIdentityConfig>();
            var enumerable = (IEnumerable<PlayerIdentityConfig>)configs;

            if (allowAnonymousIdentities)
            {
                // Да это кринж, зато код ниже не повторяется
                var defaultEnumerable = new[] { defaultConfig };
                enumerable = enumerable.Concat(defaultEnumerable);
            }

            foreach (var config in enumerable)
            {
                if (!dict.TryAdd(config.AssetHashCode, config))
                    Debug.LogWarning($"In {nameof(IdentitiesConfig)} you have repeated " +
                                     $"items, remove them or create new one", this);
            }
            
            return dict;
        }

        //public IEnumerable<PlayerAppearanceConfig> AppearancesToEnumerable() => appearances;
        public Dictionary<int, PlayerAppearanceConfig> AppearancesToDictionary()
        {
            var length = appearances.Length + configs.Length;
            var dict = new Dictionary<int, PlayerAppearanceConfig>(length);
            foreach (var appearance in appearances)
                dict.TryAdd(appearance.AssetHashCode, appearance);
            foreach (var config in configs)
                dict.TryAdd(config.Appearance.AssetHashCode, config.Appearance);
            return dict;
        } 
        #endregion
    }
}