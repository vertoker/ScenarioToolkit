using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ScenarioToolkit.Shared.VRF.Utilities
{
    public enum LogMessageType
    {
        Message,
        Warning,
        Error
    }
    
    /// <summary>
    /// Класс, расширяющий возможности стандартных логов Unity
    /// </summary>
    public static class VrfLog
    {
        public static void Print(string message, LogMessageType type = LogMessageType.Message)
        {
            switch (type)
            {
                case LogMessageType.Message: Debug.Log(message); break;
                case LogMessageType.Warning: Debug.LogWarning(message); break;
                case LogMessageType.Error: Debug.LogError(message); break;
                default: throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
        public static void Print(string message, Object context, LogMessageType type = LogMessageType.Message)
        {
            switch (type)
            {
                case LogMessageType.Message: Debug.Log(message, context); break;
                case LogMessageType.Warning: Debug.LogWarning(message, context); break;
                case LogMessageType.Error: Debug.LogError(message, context); break;
                default: throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
        
        /// <summary>
        /// Выводит соодщение в консоль при соблюдении условия
        /// </summary>
        /// <returns>condition, который получает в качестве параметра</returns>
        public static bool IsTrue(bool condition, string message, LogMessageType type = LogMessageType.Warning)
        {
            if (condition) Print(message, type);
            return condition;
        }
        
        /// <summary>
        /// Выводит соодщение в консоль при соблюдении условия, указывая контекст
        /// </summary>
        /// <returns>condition, который получает в качестве параметра</returns>
        public static bool IsTrue(bool condition, string message, Object context, LogMessageType type = LogMessageType.Warning)
        {
            if (condition) Print(message, context, type);
            return condition;
        }
        
        /// <summary>
        /// Выводит сообщение в консоль, если объект - null
        /// </summary>
        public static bool IsNull<T>(T obj, string message) where T : class => IsTrue(IsNull(obj), message);
        private static bool IsNull<T>(T obj) where T : class => obj != null && obj.Equals(null);
        
        /// <summary>
        /// Выводит сообщение в консоль, если объект - null или массив объектов пустой, указывая контекст
        /// </summary>
        public static bool IsNullOrEmpty<T>(T[] obj, string message, Object context, LogMessageType type = LogMessageType.Warning) where T : class
            => IsTrue(IsNull(obj) || obj.Length == 0, message, context, type);
        
        /// <summary>
        /// Выводит сообщение в консоль, если объект - null или массив объектов пустой
        /// </summary>
        public static bool IsNullOrEmpty<T>(T[] obj, string message, LogMessageType type = LogMessageType.Warning) where T : class
            => IsTrue(IsNull(obj) || obj.Length == 0, message, type);
        
        /// <summary>
        /// Выводит сообщение в консоль, если объект - null или список объектов пустой, указывая контекст
        /// </summary>
        public static bool IsNullOrEmpty<T>(List<T> obj, string message, Object context, LogMessageType type = LogMessageType.Warning) where T : class
            => IsTrue(IsNull(obj) || obj.Count == 0, message, context, type);
        
        /// <summary>
        /// Выводит сообщение в консоль, если объект - null или список объектов пустой
        /// </summary>
        public static bool IsNullOrEmpty<T>(List<T> obj, string message, LogMessageType type = LogMessageType.Warning) where T : class
            => IsTrue(IsNull(obj) || obj.Count == 0, message, type);
    }
}