using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Scenario.Core.Model.Interfaces;
using UnityEngine;

namespace Scenario.Utilities
{
    public static class Reflection
    {
        public static readonly Type[] AllTypes = AppDomain.CurrentDomain
            .GetAssemblies().SelectMany(a => a.GetTypes()).ToArray();
        
        public static IEnumerable<Type> GetImplementations<T>() => GetImplementations(typeof(T));
        public static IEnumerable<Type> GetImplementations(Type type) => GetImplementations(new[] { type });

        private static IEnumerable<Type> GetImplementations(IReadOnlyList<Type> toImplement)
        {
            return AllTypes.Where(FilterImpl).Except(toImplement);
            bool FilterImpl(Type type) => !type.IsAbstract && toImplement.Any(i => i.IsAssignableFrom(type));
        }

        public static IEnumerable<T> GetInstances<T>()
        {
            var implementations = GetImplementations<T>();
            return GetInstances<T>(implementations);
        }
        public static IEnumerable<T> GetInstances<T>(IEnumerable<Type> implementations)
        {
            var objects = implementations.Select(Activator.CreateInstance);
            var instances = objects.Select(o => (T)o);
            return instances;
        }

        public static FieldInfo[] GetComponentFields(this IScenarioComponent component) =>
            GetComponentFields(component.GetType());
        public static FieldInfo[] GetComponentFields(Type componentType)
        {
            var fields = componentType.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetField);
            return fields;
        }
        
        public static IEnumerable<Type> GetInheritanceHierarchy(this Type type)
        {
            for (var current = type; current != null; current = current.BaseType)
                yield return current;
        }
    }
}