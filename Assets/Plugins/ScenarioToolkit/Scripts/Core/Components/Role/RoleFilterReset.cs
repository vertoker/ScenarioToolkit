using Scenario.Core.Model.Interfaces;
using Scenario.Core.Player.Roles;
using Scenario.Utilities.Attributes;

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
    [ScenarioMeta("Сбрасывает global include и exclude фильтры", typeof(RoleFilterService))]
    public struct RoleFilterReset : IMetaComponent
    {
        
    }
}