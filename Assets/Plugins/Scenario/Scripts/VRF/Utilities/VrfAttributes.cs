using System;
using System.Linq;
#if UNITY_EDITOR

#else
using VRF.Utilities.Exceptions;
#endif

namespace VRF.Utilities
{
    /// <summary>
    /// Класс с методами для работы с атрибутами
    /// </summary>
    public static class VrfAttributes
    {
        /// <summary>
        /// Получить атрибут, примененный к данному типу
        /// </summary>
        /// <param name="type"></param>
        /// <param name="inherit">Искать ли атрибут в цепочке наследования</param>
        /// <typeparam name="TAttribute"></typeparam>
        /// <returns></returns>
        public static TAttribute GetAttribute<TAttribute>(this Type type, bool inherit = false)
        {
#if UNITY_EDITOR
            return (TAttribute)type.GetCustomAttributes(inherit).FirstOrDefault(o => o is TAttribute);
#else
            throw new OnlyUnityEditorException();
#endif
        }

        /// <summary>
        /// Имеет ли данный тип атрибут
        /// </summary>
        /// <param name="type"></param>
        /// <param name="inherit">Искать ли атрибут в цепочке наследования</param>
        /// <typeparam name="TAttribute"></typeparam>
        /// <returns></returns>
        public static bool HasAttribute<TAttribute>(this Type type, bool inherit = false)
        {
#if UNITY_EDITOR
            return type.GetCustomAttributes(inherit).Any(o => o is TAttribute);
#else
            throw new OnlyUnityEditorException();
#endif
        }

        /// <summary>
        /// Попробовать получить атрибут, примененный к данному типу
        /// </summary>
        /// <param name="type"></param>
        /// <param name="attribute"></param>
        /// <param name="inherit">Искать ли атрибут в цепочке наследования</param>
        /// <typeparam name="TAttribute"></typeparam>
        /// <returns>true и attribute, если он найден, и false, если не найден</returns>
        public static bool TryGetAttribute<TAttribute>(this Type type, out TAttribute attribute, bool inherit = false)
        {
            var obj = type.GetCustomAttributes(inherit).FirstOrDefault(o => o is TAttribute);
            if (obj == null)
            {
                attribute = default;
                return false;
            }
            attribute = (TAttribute)obj;
            return true;
        }
    }
}