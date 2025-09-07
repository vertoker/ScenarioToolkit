using System;
using UnityEngine;

namespace ScenarioToolkit.Editor.Content.Fields.Experimental.Lists
{
    /// <summary>
    /// Незаконченная система для отрисовки массива полей
    /// </summary>
    public abstract class BaseListScriptableObject : ScriptableObject
    {
        public abstract string GetListName();
        public abstract Type GetElementType();
    }
}