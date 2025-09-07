using System;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Scenario.Utilities;
using ScenarioToolkit.Shared;

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
    /// <summary>
    /// Особенный объект, который позволяет сериализовать объект через JSON,
    /// при этом самостоятельно определять тип и логику сериализации.
    /// Подробнее в ObjectTypedConverter
    /// </summary>
    public readonly struct ObjectTyped
    {
        public static readonly ObjectTyped Empty = new(null, TypesReflection.SerializationNullType);
        
        [JsonProperty] public readonly Type Type;
        [JsonProperty] [CanBeNull] public readonly object Object;

        private ObjectTyped(object obj, Type type)
        {
            Object = obj;
            Type = type;
        }
        private ObjectTyped(object obj)
        {
            Object = obj;
            Type = obj.IsNullOrDestroyed() 
                ? TypesReflection.SerializationNullType : obj.GetType();
        }

        /// <summary> Объект МОЖЕТ быть сохранён как null, СО спецификацией типа </summary>
        public static ObjectTyped ConstructNull(object obj, Type type) => new(obj, type);

        /// <summary> Объект НЕ МОЖЕТ быть сохранён как null, иначе он сохранится как Empty, БЕЗ спецификации типа </summary>
        public static ObjectTyped ConstructNotNull(object obj) => new(obj);

        public bool IsEmpty() => Equals(this, Empty);

        public T GetAs<T>()
        {
            if (Object == null)
                return default;
            return (T)Object;
        }
        public bool Is<T>() => Type == typeof(T);
    }
}