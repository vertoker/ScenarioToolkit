using System.Collections.Generic;
using Scenario.Core.Model.Interfaces;

// Previous: ComponentVariablesV2
//  Current: ComponentVariablesV6
//     Next: 

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
    public class ComponentVariablesV6 : IComponentVariables
    {
        public List<IMemberVariable> MemberVariables { get; set; } = new();
        
        public int Insert(string memberName, string variableName)
        {
            var index = IndexOf(memberName);
            if (index != -1)
            {
                var temp = MemberVariables[index];
                temp.VariableName = variableName;
                MemberVariables[index] = temp;
            }
            else
            {
                var memberVariable = IMemberVariable.CreateNew();
                memberVariable.MemberName = memberName;
                memberVariable.VariableName = variableName;
                MemberVariables.Add(memberVariable);
            }
            return index;
        }
        public int Insert(IMemberVariable memberVariable)
            => Insert(memberVariable.MemberName, memberVariable.VariableName);
        
        public int Remove(string memberName)
        {
            var index = IndexOf(memberName);
            if (index != -1)
                MemberVariables.RemoveAt(index);
            return index;
        }
        public void Clear()
        {
            MemberVariables.Clear();
        }

        public int IndexOf(string memberName)
        {
            var length = MemberVariables.Count;
            for (var i = 0; i < length; i++)
                if (MemberVariables[i].MemberName == memberName)
                    return i;
            return -1;
        }
        public bool Contains(string memberName)
        {
            var index = IndexOf(memberName);
            return index != -1;
        }
        public IMemberVariable GetValueOrDefault(string memberName)
        {
            var index = IndexOf(memberName);
            return index == -1 ? null : MemberVariables[index];
        }
        public bool TryGet(string memberName, out IMemberVariable memberVariable)
        {
            var index = IndexOf(memberName);
            if (index == -1)
            {
                memberVariable = null;
                return false;
            }
            memberVariable = MemberVariables[index];
            return true;
        }
    }
}