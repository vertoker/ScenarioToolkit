using JetBrains.Annotations;
using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Core.Player.Roles;
using ScenarioToolkit.Shared.Attributes;
using ScenarioToolkit.Shared.VRF;

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
    [ScenarioMeta("Добавляет в local include фильтр этот identity. " +
                  "Работает только для этой ноды. Может вызываться множественно", typeof(RoleFilterService))]
    public struct RoleNodeFilter : IMetaComponent, IComponentDefaultValues
    {
        [CanBeNull] public PlayerIdentityConfig Identity;
        
        public void SetDefault()
        {
            Identity = null;
        }
    }
}