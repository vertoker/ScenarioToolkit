using Scenario.Core.Model.Interfaces;

// Previous: MemberVariableV2
//  Current: MemberVariableV6
//     Next: 

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
    public class MemberVariableV6 : IMemberVariable
    {
        public string MemberName { get; set; }
        public string VariableName { get; set; }
    }
}