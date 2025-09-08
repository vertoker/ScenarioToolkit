using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Shared.Extensions;

namespace ScenarioToolkit.Shared
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public static class ComponentsReflection
    {
        public static readonly Type[] AllComponentTypes = Reflection.GetImplementations<IScenarioComponent>().ToArray();
        //public static readonly HashSet<Type> OnlyLocalComponentTypes = AllComponentTypes.Where(t => t.HasAttribute(t)).ToHashSet();

        public static bool IsValid(this IScenarioComponent component, bool isHost)
        {
            if (!isHost && component is IScenarioOnlyHost)
                return false;
            return component is not IScenarioIgnore;
        }
        
        public static string ToGraphString(this IScenarioComponent component)
        {
            if (component == null) return "<b>null</b>\n";
            var fields = component.GetComponentFields();
            var values = fields.Select(field =>
            {
                var value = component.GetValueByField(field);
                return value.GetScenarioFieldContent();
            }).Select(s => $"   {s}");
            return $"<b>{component.GetType().PrettyName()}</b>:\n{string.Join("\n", values)}";
        }
        public static string ToGraphString(this IScenarioComponent component, [CanBeNull] IComponentVariables overrides)
        {
            var fields = component.GetComponentFields();
            var values = fields.Select(field =>
            {
                var memberVariable = overrides?.GetValueOrDefault(field.Name);
                if (memberVariable != null)
                    return $"<b>{memberVariable.VariableName}</b>";

                var value = component.GetValueByField(field);
                return value.GetScenarioFieldContent();
            }).Select(s => $"   {s}");
            
            return $"<b>{component.GetType().PrettyName()}</b>:\n{string.Join("\n", values)}";
        }
        
        public static IEnumerable<object> GetValues(this IScenarioComponent component)
        {
            var members = component.GetComponentFields();
            var values = members.Select(component.GetValueByField);
            return values;
        }
        
        public static object GetValueByField(this IScenarioComponent instance, FieldInfo fieldInfo) => fieldInfo.GetValue(instance);
        public static void SetValueByField(this IScenarioComponent instance, FieldInfo fieldInfo, object value) => fieldInfo.SetValue(instance, value);
    }
}