using UnityEngine;
using Object = UnityEngine.Object;

namespace ScenarioToolkit.Shared
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public static class AssertLog
    {
        public static bool NotNull<TComponent>(Object field, string nameField)
        {
            if (field) return false;
            Debug.LogWarning($"Empty object {nameField} in {typeof(TComponent).Name}, drop");
            return true;
        }
        public static bool NotNull<TComponent>(object field, string nameField)
        {
            if (field != null) return false;
            Debug.LogWarning($"Empty object {nameField} in {typeof(TComponent).Name}, drop");
            return true;
        }
        
        public static bool NotEmpty<TComponent>(string field, string nameField)
        {
            if (string.IsNullOrEmpty(field))
            {
                Debug.LogWarning($"Empty string {nameField} in {typeof(TComponent).Name}, drop");
                return true;
            }
            return false;
        }
        public static bool NotWhiteSpace<TComponent>(string field, string nameField)
        {
            if (string.IsNullOrWhiteSpace(field))
            {
                Debug.LogWarning($"Empty or white space {nameField} in {typeof(TComponent).Name}, drop");
                return true;
            }
            return false;
        }
        
        public static bool IsTrue(bool condition, string message, Object context = null)
        {
            if (condition) return false;
            Debug.LogWarning(message, context);
            return true;
        }
        public static bool IsFalse(bool condition, string message, Object context = null)
        {
            if (!condition) return false;
            Debug.LogWarning(message, context);
            return true;
        }
        
        // >
        public static bool Above<TComponent>(float value, float toAbove, string nameField)
            => Above<TComponent>(value <= toAbove, value, toAbove, nameField);
        public static bool Above<TComponent>(int value, int toAbove, string nameField)
            => Above<TComponent>(value <= toAbove, value, toAbove, nameField);
        
        private static bool Above<TComponent>(bool condition, object value, object toAbove, string nameField)
        {
            if (condition)
            {
                Debug.LogWarning($"Value {nameField} in {typeof(TComponent).Name} " +
                                 $"must be above {toAbove}, but it equals {value}, drop");
                return true;
            }
            return false;
        }
        
        // <
        public static bool Below<TComponent>(float value, float toBelow, string nameField)
            => Below<TComponent>(value >= toBelow, value, toBelow, nameField);
        public static bool Below<TComponent>(int value, int toBelow, string nameField)
            => Below<TComponent>(value >= toBelow, value, toBelow, nameField);
        
        private static bool Below<TComponent>(bool condition, object value, object toBelow, string nameField)
        {
            if (condition)
            {
                Debug.LogWarning($"Value {nameField} in {typeof(TComponent).Name} " +
                                 $"must be below {toBelow}, but it equals {value}, drop");
                return true;
            }
            return false;
        }
        
        // >=
        public static bool AboveEqual<TComponent>(float value, float toAboveEqual, string nameField)
            => AboveEqual<TComponent>(value < toAboveEqual, value, toAboveEqual, nameField);
        public static bool AboveEqual<TComponent>(int value, int toAboveEqual, string nameField)
            => AboveEqual<TComponent>(value < toAboveEqual, value, toAboveEqual, nameField);
        
        private static bool AboveEqual<TComponent>(bool condition, object value, object toAboveEqual, string nameField)
        {
            if (condition)
            {
                Debug.LogWarning($"Value {nameField} in {typeof(TComponent).Name} " +
                                 $"must be above or equal {toAboveEqual}, but it equals {value}, drop");
                return true;
            }
            return false;
        }
        
        // <=
        public static bool BelowEqual<TComponent>(float value, float toBelowEqual, string nameField)
            => BelowEqual<TComponent>(value > toBelowEqual, value, toBelowEqual, nameField);
        public static bool BelowEqual<TComponent>(int value, int toBelowEqual, string nameField)
            => BelowEqual<TComponent>(value > toBelowEqual, value, toBelowEqual, nameField);
        
        private static bool BelowEqual<TComponent>(bool condition, object value, object toBelowEqual, string nameField)
        {
            if (condition)
            {
                Debug.LogWarning($"Value {nameField} in {typeof(TComponent).Name} " +
                                 $"must be below or equal {toBelowEqual}, but it equals {value}, drop");
                return true;
            }
            return false;
        }
    }
}