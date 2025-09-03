using Scenario.Core.Model;
using Scenario.Core.Model.Interfaces;

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model.Interfaces
{
    /// <summary>
    /// Обычный бинд для перезаписей полей компонента, который использует имя поля и имя переменной
    /// </summary>
    public interface IMemberVariable : IModelReflection<MemberVariableV6, IMemberVariable>
    {
        public string MemberName { get; set; }
        public string VariableName { get; set; }
    }
}