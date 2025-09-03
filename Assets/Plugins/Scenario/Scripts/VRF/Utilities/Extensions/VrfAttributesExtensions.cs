using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VRF.Utilities.Extensions
{
    /// <summary>
    /// Расширения для проверки наличия атрибута у компонентов
    /// </summary>
    public static class VrfAttributesExtensions
    {
        public static bool HasAttribute<TAttribute>(this Component obj, bool inherit = false)
            where TAttribute : Attribute
            => obj.GetType().UnderlyingSystemType.GetCustomAttributes(inherit).Any(o => o is TAttribute);
        
        public static bool AnyHasAttribute<TAttribute>(this IEnumerable<Component> objs, bool inherit = false)
            where TAttribute : Attribute
            => objs.Any(o => o.HasAttribute<TAttribute>(inherit));
        
        public static bool AllHasAttribute<TAttribute>(this IEnumerable<Component> objs, bool inherit = false)
            where TAttribute : Attribute
            => objs.All(o => o.HasAttribute<TAttribute>(inherit));
    }
}