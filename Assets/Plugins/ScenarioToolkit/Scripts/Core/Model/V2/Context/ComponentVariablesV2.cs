using System.Collections.Generic;
using Scenario.Core.Model.Interfaces;

// Previous: 
//  Current: ComponentVariablesV2
//     Next: ComponentVariablesV6

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
    public class ComponentVariablesV2
    {
        public List<MemberVariableV2> MemberVariables { get; set; } = new();
    }
}