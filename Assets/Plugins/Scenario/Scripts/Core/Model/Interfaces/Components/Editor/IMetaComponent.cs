// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model.Interfaces
{
    /// <summary>
    /// Внутренний интерфейс для обозначения компонент,
    /// которые должны иметь возможность быть везде,
    /// но при этом не взаимодействовать со сценарной шиной событий
    /// </summary>
    public interface IMetaComponent : IScenarioAction, IScenarioCondition, IScenarioIgnore
    {
        
    }
}