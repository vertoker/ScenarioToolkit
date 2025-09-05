using System;
using System.Collections.Generic;
using System.Linq;
using Scenario.Utilities.Extensions;

namespace Scenario.Utilities
{
    public static class ScenarioTypeParser
    {
        public const string TypePropertyName = "$type";
        
        private static readonly Type[] Types = AppDomain.CurrentDomain
            .GetAssemblies().SelectMany(a => a.GetTypes()).ToArray();
        private static readonly Dictionary<string, Type> Replacements = new();
        
        public static string Serialize(Type type)
        {
            return $"{type.FullName}, {type.Assembly.GetName().Name}";
        }
        public static string Serialize(object value)
        {
            return value == null ? "None" : Serialize(value.GetType());
        }
        
        public static bool TryDeserialize(string serializedType, out Type type)
        {
            type = Type.GetType(serializedType) ?? FindByName(serializedType);
            return type != null;
        }
        private static Type FindByName(string serializedType)
        {
            var shortName = serializedType.Split(',')[0].Split('.').Last();
            if (Replacements.TryGetValue(serializedType, out var type))
                return type;
            
            var options = Types.Where(t => t.Name == shortName).ToList();
            options.Sort((a, b) => a.AssemblyQualifiedName.LevenshteinDistance(serializedType, 100)
                .CompareTo(b.AssemblyQualifiedName.LevenshteinDistance(serializedType, 100)));

            if (options.Count <= 0 || options[0].AssemblyQualifiedName == null)
            {
                //Debug.LogWarning($"Type {shortName} not found");
                return null;
            }

            var result = options[0];
            Replacements.Add(serializedType, result);
            return result;
        }
    }
}