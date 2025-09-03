using System.Collections.Generic;
using Scenario.Core.Model;
using Scenario.Core.Model.Interfaces;

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model.Interfaces
{
    /// <summary>
    /// Список перезаписей полей конкретного компонента с использованием переменных
    /// </summary>
    public interface IComponentVariables : IModelReflection<ComponentVariablesV6, IComponentVariables>
    {
        public List<IMemberVariable> MemberVariables { get; set; }

        public int Insert(string memberName, string variableName);
        public int Insert(IMemberVariable memberVariable);
        public int Remove(string memberName);
        public void Clear();

        public int IndexOf(string memberName);
        public bool Contains(string memberName);
        public IMemberVariable GetValueOrDefault(string memberName);
        public bool TryGet(string memberName, out IMemberVariable memberVariable);
    }
}