using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ScenarioToolkit.Shared.VRF
{
    /// <summary>
    /// Все данные ролей игроков, сильно связан с сетью. У всех сетевых игроков есть его локальная копия
    /// </summary>
    [CreateAssetMenu(fileName = nameof(IdentitiesConfig), menuName = "VRF/Identities/" + nameof(IdentitiesConfig))]
    public class IdentitiesConfig : ScriptableObject
    {
        [Header("Identities")]
        [SerializeField] private PlayerIdentityConfig[] configs;
        
        // Если игрок не может авторизоваться, то ему отправляется анонимная роль, иначе просто отключает
        [SerializeField] private bool allowAnonymousIdentities = true;
        
        [SerializeField] private PlayerIdentityConfig defaultConfig;
        
        [Header("Register")]
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