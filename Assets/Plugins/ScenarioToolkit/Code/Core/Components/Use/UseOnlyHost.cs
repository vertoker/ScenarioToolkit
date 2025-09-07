using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Shared.Attributes;

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