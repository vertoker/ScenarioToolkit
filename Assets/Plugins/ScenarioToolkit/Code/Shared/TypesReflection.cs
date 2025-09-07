using System;
using Scenario.Core.Model;
using Object = UnityEngine.Object;

namespace ScenarioToolkit.Shared
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public static class TypesReflection
    {
        // Это максимально тупой, но рабочий способ обозначить null type, не используя null,
        // так как в рамках десериализации, тип object абсолютно не имеет смысла
        public static readonly Type SerializationNullType = typeof(object);
        
        public static readonly bool FallbackValue = false;
        public static readonly Type FallbackType = typeof(bool);
        public static readonly ObjectTyped FallbackTypedValue 
            = ObjectTyped.ConstructNull(FallbackValue, FallbackType);

        public const int MaxLength = 30;
        
        public static bool IsNullOrDestroyed(this object obj)
        {
            return obj switch
            {
                null => true,
                Object unityObj => !unityObj,
                _ => false
            };
        }
        public static string GetScenarioFieldContent(this object obj)
        {
            return obj switch
            {
                null => "null",
                Object unityObj when !unityObj => "missing",
                Object unityObj when unityObj => unityObj.name,
                ObjectTyped typed => $"<b>obj</b>: {typed.Object.GetScenarioFieldContent()}",
                NodeRef nodeLink => $"<b>node</b>: {nodeLink.Hash}",
                _ => obj.ToString().TruncateEllipsis(MaxLength),
            };
        }
        
        // https://stackoverflow.com/questions/2776673/how-do-i-truncate-a-net-string
        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value[..maxLength]; 
        }
        // Скопировано из TMP_Text, ставит многоточие если превышен лимит (без учёта шрифта, многоточие - 3 точки)
        public static string TruncateEllipsis(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : $"{value[..(maxLength - 3)]}..."; 
        }
    }
}