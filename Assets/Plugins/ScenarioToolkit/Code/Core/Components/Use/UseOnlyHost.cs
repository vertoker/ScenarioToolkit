using Scenario.Core.Model.Interfaces;
using Scenario.Core.Network;
using Scenario.Utilities.Attributes;

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
    [ScenarioMeta("Запрещает исполнение ноды (на которой находится) везде кроме хоста", 
        typeof(IScenarioOnlyHost))]
    public struct UseOnlyHost : IMetaComponent
    {
        public static readonly IMetaComponent Self = new UseOnlyHost();
    }
}