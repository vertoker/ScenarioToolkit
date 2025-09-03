using System;
using System.Collections.Generic;
using System.Linq;

namespace VRF.Editor.MethodTools.Services
{
    public class TypesCacheEditor
    {
        private static Type[] _types;

        public static IReadOnlyList<Type> Types => _types;

        public static IReadOnlyList<Type> GetLazyTypes()
        {
            if (_types == null)
                OnScriptReload();
            return _types;
        }
        
        [UnityEditor.Callbacks.DidReloadScripts]
        public static void OnScriptReload()
        {
            _types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes()).ToArray();
        }
    }
}