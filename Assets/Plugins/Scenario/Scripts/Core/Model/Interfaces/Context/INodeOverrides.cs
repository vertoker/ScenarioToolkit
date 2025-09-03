using System.Collections.Generic;
using JetBrains.Annotations;

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model.Interfaces
{
    /// <summary>
    /// Список всех перезаписанных полей во всех компонентах сценария. Реализация
    /// не самая чистая, зато эти данные просто накладываются поверх компонентов перед исполнением 
    /// </summary>
    public interface INodeOverrides
    {
        // Так как компоненты - это обычный список, то List<IComponentVariables> имеет особенности
        // - Его длина всегда равна длине компонентов
        // - Если у компонента нет перезаписей, то вместо экземпляра IComponentVariables будет null
        // - Если во всей ноде нет перезаписей, то списка просто не будет в словаре
        // Все эти правила записи соблюдает редактор переменных, подробнее туда (NodeOverridesController)
        
        // node hash - components [ members [ member name - variable name ] ]
        [CanBeNull] public Dictionary<int, List<IComponentVariables>> NodeOverrides { get; set; }
    }
}