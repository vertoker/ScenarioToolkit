using System;
using System.Collections.Generic;
using System.Linq;
using ScenarioToolkit.Shared.Extensions;

namespace ScenarioToolkit.Shared.Exceptions
{
    public static class Assert
    {
        public static void That(bool condition)
        {
            if (!condition)
            {
                throw CreateException("Assert hit!");
            }
        }
        public static void That(bool condition, string message)
        {
            if (!condition)
            {
                throw CreateException(message);
            }
        }
        
        public static void IsNotEmpty(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw CreateException("Unexpected null or empty string");
            }
        }
        
        // This is better because IsEmpty with IEnumerable causes a memory alloc
        public static void IsEmpty<T>(IReadOnlyCollection<T> list)
        {
            if (list.Count != 0)
            {
                throw CreateException($"Expected collection to be empty but instead found '{list.Count}' elements");
            }
        }
        
        public static void IsType<T>(object obj)
        {
            IsType<T>(obj, "");
        }
        public static void IsType<T>(object obj, string message)
        {
            if (obj is not T)
            {
                throw CreateException($"Assert Hit! {message}\n" +
                                      $"Wrong type found. Expected '{typeof(T).PrettyName()}' (left) " +
                                      $"but found '{obj.GetType().PrettyName()}' (right). ");
            }
        }
        public static void DerivesFrom<T>(Type type)
        {
            if (!type.DerivesFrom<T>())
            {
                throw CreateException($"Expected type '{type.Name}' to derive from '{typeof(T).Name}'");
            }
        }
        public static void DerivesFromOrEqual<T>(Type type)
        {
            if (!type.DerivesFromOrEqual<T>())
            {
                throw CreateException($"Expected type '{type.Name}' to derive from or be equal to '{typeof(T).Name}'");
            }
        }
        public static void DerivesFrom(Type childType, Type parentType)
        {
            if (!childType.DerivesFrom(parentType))
            {
                throw CreateException($"Expected type '{childType.Name}' to derive from '{parentType.Name}'");
            }
        }
        public static void DerivesFromOrEqual(Type childType, Type parentType)
        {
            if (!childType.DerivesFromOrEqual(parentType))
            {
                throw CreateException($"Expected type '{childType.Name}' to derive from or be equal to '{parentType.Name}'");
            }
        }
        public static void IsEqual(object left, object right)
        {
            IsEqual(left, right, string.Empty);
        }
        public static void IsEqual(object left, object right, Func<string> messageGenerator)
        {
            if (!object.Equals(left, right))
            {
                left = left ?? "<NULL>";
                right = right ?? "<NULL>";
                throw CreateException($"Assert Hit! {messageGenerator()}.  Expected '{left}' (left) but found '{right}' (right). ");
            }
        }
        public static void IsApproximately(float left, float right, float epsilon = 0.00001f)
        {
            bool isEqual = Math.Abs(left - right) < epsilon;

            if (!isEqual)
            {
                throw CreateException($"Assert Hit! Expected '{left}' (left) but found '{right}' (right). ");
            }
        }
        public static void IsEqual(object left, object right, string message)
        {
            if (!object.Equals(left, right))
            {
                left = left ?? "<NULL>";
                right = right ?? "<NULL>";
                throw CreateException($"Assert Hit! {message}\nExpected '{left}' (left) but found '{right}' (right). ");
            }
        }
        
        public static void IsNotEqual(object left, object right)
        {
            IsNotEqual(left, right, string.Empty);
        }
        public static void IsNotEqual(object left, object right, Func<string> messageGenerator)
        {
            if(object.Equals(left, right))
            {
                left = left ?? "<NULL>";
                right = right ?? "<NULL>";
                throw CreateException($"Assert Hit! {messageGenerator()}\nExpected '{left}' (left) to differ from '{right}' (right). ");
            }
        }
        public static void IsNotEqual(object left, object right, string message)
        {
            if(object.Equals(left, right))
            {
                left = left ?? "<NULL>";
                right = right ?? "<NULL>";
                throw CreateException($"Assert Hit! {message}\nExpected '{left}' (left) to differ from '{right}' (right). ");
            }
        }
        
        public static void IsNull(object val)
        {
            if (val != null)
            {
                throw CreateException($"Assert Hit! Expected null pointer but instead found '{val}'");
            }
        }
        public static void IsNull(object val, string message)
        {
            if (val != null)
            {
                throw CreateException($"Assert Hit! {message}");
            }
        }
        public static void IsNotNull(object val, string message)
        {
            if (val == null)
            {
                throw CreateException($"Assert Hit! {message}");
            }
        }
        
        public static ScenarioException CreateException()
        {
            return new ScenarioException("Assert hit!");
        }
        public static ScenarioException CreateException(string message)
        {
            return new ScenarioException(message);
        }
    }
}