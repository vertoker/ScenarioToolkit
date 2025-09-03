using System.Linq;
using Scenario.Core.Model.Interfaces;
using Scenario.Utilities;

namespace Scenario.Editor.Utilities
{
    public static class VariablesReflection
    {
        public static string ToGraphString(this IVariableEnvironment variableEnvironment)
        {
            if (variableEnvironment.Variables == null) return string.Empty;
            
            var values = variableEnvironment.Variables.Select(pair =>
            {
                var value = pair.Value.Object.GetScenarioFieldContent();
                return $"{pair.Key}: {value}";
            }).Select(s => $"   {s}");
            
            return $"<b>Variables</b>:\n{string.Join("\n", values)}";
        }
    }
}