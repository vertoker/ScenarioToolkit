using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Core.Player;
using ScenarioToolkit.Shared.Attributes;

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
    [ScenarioMeta("Особенная нода, применяемая как модификатор исходного алгоритма, вынуждает переходить дальше если " +
                  "будет исполнен хотя бы один Action или будет выполнено хотя-бы одно Condition", typeof(ScenarioPlayer))]
    public struct UseOr : IMetaComponent
    {
        
    }
}