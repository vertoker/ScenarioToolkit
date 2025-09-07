using JetBrains.Annotations;
using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Core.Player.Roles;
using ScenarioToolkit.Shared.Attributes;
using ScenarioToolkit.Shared.VRF;

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
    [ScenarioMeta("Добавляет в global include фильтр этот identity. Может вызываться множественно", typeof(RoleFilterService))]
    public struct RoleFilter : IMetaComponent, IComponentDefaultValues
    {
        [CanBeNull] public PlayerIdentityConfig Identity;
        
        public void SetDefault()
        {
            Identity = null;
        }
    }
}