using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Scenario.Core.Model;
using Scenario.Editor.Content.Fields.Base;
using Scenario.Editor.Content.Fields.Types;
using Scenario.Editor.Content.Fields.Types.Convertible;
using Scenario.Editor.Content.Fields.Types.Custom;
using Scenario.Editor.Content.Fields.Types.Unity;
using Scenario.Utilities;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Scenario.Editor.Content.Fields
{
    /// <summary>
    /// Помощник по информации и в создании полей сценария
    /// </summary>
    public static class ScenarioFields
    {
        private static readonly Dictionary<Type, CreatorEntry> TypesToCreators = new();
        private static readonly Dictionary<Type, ScenarioFieldType> TypesToEnums = new();
        private static readonly Dictionary<ScenarioFieldType, Type> EnumsToTypes = new();
        
        public static readonly ScenarioFieldType FallbackSelectedType;

        private static void Bind(Type type, ITypeFieldCreator definedCreator) // enum = null
        {
            var entry = new CreatorEntry(definedCreator);
            TypesToCreators.Add(type, entry);
        }
        private static void Bind(ScenarioFieldType enumType, Type type, 
            ITypeFieldCreator definedCreator)
        {
            var entry = new CreatorEntry(definedCreator);
            TypesToCreators.Add(type, entry);
            TypesToEnums.Add(type, enumType);
            EnumsToTypes.Add(enumType, type);
        }
        private static void Bind(ScenarioFieldType enumType, Type type, 
            ITypeFieldCreator definedCreator, ITypeFieldCreator undefinedCreator)
        {
            var entry = new CreatorEntry(definedCreator, undefinedCreator);
            TypesToCreators.Add(type, entry);
            TypesToEnums.Add(type, enumType);
            EnumsToTypes.Add(enumType, type);
        }
        static ScenarioFields()
        {
            // Core
            Bind(ScenarioFieldType.String, typeof(string), new StringFieldCreator());
            Bind(ScenarioFieldType.UObject, typeof(Object), new UObjectFieldCreator());
            Bind(ScenarioFieldType.Int, typeof(int), new IntFieldCreator());
            Bind(ScenarioFieldType.Float, typeof(float), new FloatFieldCreator());
            // это особенный бинд, так как тип enum я явно не знаю и его нужно выбирать (это уже сам creator)
            Bind(ScenarioFieldType.Enum, typeof(Enum), new EnumFieldCreator(), new EnumUndefinedFieldCreator());
            Bind(ScenarioFieldType.Bool, typeof(bool), new BoolFieldCreator());
            
            // Unity
            Bind(ScenarioFieldType.Vector2, typeof(Vector2), new Vector2FieldCreator());
            Bind(ScenarioFieldType.Vector3, typeof(Vector3), new Vector3FieldCreator());
            Bind(ScenarioFieldType.Vector4, typeof(Vector4), new Vector4FieldCreator());
            Bind(ScenarioFieldType.Color, typeof(Color), new ColorFieldCreator());
            Bind(ScenarioFieldType.Vector2Int, typeof(Vector2Int), new Vector2IntFieldCreator());
            Bind(ScenarioFieldType.Vector3Int, typeof(Vector3Int), new Vector3IntFieldCreator());
            Bind(ScenarioFieldType.Bounds, typeof(Bounds), new BoundsFieldCreator());
            Bind(ScenarioFieldType.Rect, typeof(Rect), new RectFieldCreator());
            Bind(ScenarioFieldType.BoundsInt, typeof(BoundsInt), new BoundsIntFieldCreator());
            Bind(ScenarioFieldType.RectInt, typeof(RectInt), new RectIntFieldCreator());
            Bind(ScenarioFieldType.Hash128, typeof(Hash128), new Hash128FieldCreator());
            
            // C#
            Bind(ScenarioFieldType.Long, typeof(long), new LongFieldCreator());
            Bind(ScenarioFieldType.Double, typeof(double), new DoubleFieldCreator());
            Bind(ScenarioFieldType.UInt, typeof(uint), new UnsignedIntegerFieldCreator());
            Bind(ScenarioFieldType.ULong, typeof(ulong), new UnsignedLongFieldCreator());
            
            // Custom Addressing
            Bind(ScenarioFieldType.NodeLink, typeof(NodeRef), new NodeLinkFieldCreator());
            
            // Convertible Fields
            Bind(ScenarioFieldType.Quaternion, typeof(Quaternion), new QuaternionFieldCreator());
            
            // No Field Type
            Bind(typeof(ObjectTyped), new ObjectTypedFieldCreator());

            FallbackSelectedType = TypesToEnums[TypesReflection.FallbackType];
        }

        // При получении есть параметр allowUndefined, который вместо обычного fieldCreator, отдаёт
        // его аналог undefinedFieldCreator если он есть
        
        public static IEnumerable<ITypeFieldCreator> GetCreators(bool allowUndefined = false)
        {
            return TypesToCreators.Values.Select(c => c.GetCreator(allowUndefined));
        }
        public static Type GetType(ScenarioFieldType enumType)
        {
            if (EnumsToTypes.TryGetValue(enumType, out var type)) return type;
            
            Debug.LogError($"Can't find type for {Enum.GetName(typeof(ScenarioFieldType), enumType)}, " +
                           $"return fallback type");
            return EnumsToTypes[FallbackSelectedType];
        }
        public static ScenarioFieldType GetFieldEnum(Type type)
        {
            //Debug.LogError($"Can't find enumType for {type.Name}, return {nameof(FallbackType)}");
            
            // TODO временное решение, добавить полноценную поддержку для наследников
            if (typeof(Object).IsAssignableFrom(type)) type = typeof(Object);
            if (typeof(Enum).IsAssignableFrom(type)) type = typeof(Enum);
            
            // При получении ObjectTyped, он возвращает string - это временное решение
            return TypesToEnums.GetValueOrDefault(type, FallbackSelectedType);
        }
        public static ITypeFieldCreator GetCreator(Type valueType, bool allowUndefined = true)
        {
            // TODO временное решение, добавить полноценную поддержку для наследников
            if (typeof(Object).IsAssignableFrom(valueType)) valueType = typeof(Object);
            if (typeof(Enum).IsAssignableFrom(valueType)) valueType = typeof(Enum);
            
            if (TypesToCreators.TryGetValue(valueType, out var entry))
                return entry.GetCreator(allowUndefined);
            
            Debug.LogError($"Can't find field for {valueType.Name}, return {nameof(FallbackSelectedType)} Creator");
            return TypesToCreators[TypesReflection.FallbackType].GetCreator(allowUndefined);
        }
        
        private readonly struct CreatorEntry
        {
            private readonly ITypeFieldCreator definedCreator;
            [CanBeNull] private readonly ITypeFieldCreator undefinedCreator;
            
            public CreatorEntry(ITypeFieldCreator definedCreator, ITypeFieldCreator undefinedCreator = null)
            {
                this.definedCreator = definedCreator;
                this.undefinedCreator = undefinedCreator;
            }
            
            public ITypeFieldCreator GetCreator(bool allowUndefined = false)
            {
                if (!allowUndefined) return definedCreator;
                return undefinedCreator ?? definedCreator;
            }
        }
    }
}