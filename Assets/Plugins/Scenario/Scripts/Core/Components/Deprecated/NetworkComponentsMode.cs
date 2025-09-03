using Scenario.Core.Model.Interfaces;
using VRF.Players.Scriptables;
using System;

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
    [Obsolete("NetV0 is not supported, use new net modules")]
    public struct NetworkComponentsMode : IMetaComponent, INotSerializableComponent, IComponentDefaultValues
    {
        public bool IsLocal;
        public bool IsRemote;
        public bool FilterByIdentity;
        public PlayerIdentityConfig Identity;
        
        public static NetworkComponentsMode Default => new()
        {
            IsLocal = true,
            IsRemote = true,
            FilterByIdentity = false,
            Identity = null
        };

        public bool IsValidIdentity(int identityCode)
        {
            // Если отсутствует фильтрация по identity
            if (!FilterByIdentity) return true;
            // Иначе сравнение под коду identity
            return identityCode == Identity.AssetHashCode;
        }

        public void SetDefault()
        {
            var @default = Default;
            IsLocal = @default.IsLocal;
            IsRemote = @default.IsRemote;
            FilterByIdentity = @default.FilterByIdentity;
            Identity = null;
        }
    }
}