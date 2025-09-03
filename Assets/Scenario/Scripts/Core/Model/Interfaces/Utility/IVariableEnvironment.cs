using System.Collections.Generic;
using Scenario.Core.Model;

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model.Interfaces
{
    /// <summary>
    /// Переменная среда это массив из переменных, имя и тип которых можно выбирать прямо в редакторе.
    /// Сам интерфейс используется для операций с переменными средами, а также под ней находятся разные реализации
    /// </summary>
    public interface IVariableEnvironment
    {
        // variable name - serialized non-generic variable value
        public Dictionary<string, ObjectTyped> Variables { get; set; }
    }
}