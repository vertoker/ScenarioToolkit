using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scenario.Editor.Content.Fields.Experimental.Lists;
using Scenario.Utilities;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Scenario.Editor.Content.Fields.Experimental.BaseList
{
    /// <summary>
    /// Незаконченная система для отрисовки массива полей
    /// </summary>
    // TODO закончить эту систему отрисовки (и не стоит забывать про сериализацию потом исправить)
    public class ListFieldCreator// : ITypeFieldCreator 
    {
        private static readonly Dictionary<Type, Type> ScriptableTypesList =
            Reflection.GetImplementations<BaseListScriptableObject>().ToDictionary(fieldType =>
            {
                var instance = (BaseListScriptableObject)ScriptableObject.CreateInstance(fieldType);
                var scriptableType = instance.GetElementType();
                Object.DestroyImmediate(instance);
                return scriptableType;
            });

        public bool CanCreate(Type valueType)
        {
            return ScriptableTypesList.Keys.Any(fieldType => fieldType.IsAssignableFrom(valueType));
        }

        public VisualElement CreateElement(object value, Type valueType, Action<object> valueChangedCallback)
        {
            var scriptableType = ScriptableTypesList[valueType];
            
            var list = (IList)value;
            if (list == null)
            {
                list = new List<GameObject>();
                valueChangedCallback?.Invoke(list);
            }

            var drawer = new ListDrawer(scriptableType);
            drawer.List.Clear();
            foreach (var item in list)
                drawer.List.Add(item);

            drawer.List.ItemsChanged += items =>
            {
                list.Clear();
                foreach (var item in items)
                    list.Add(item);
                valueChangedCallback?.Invoke(list);
            };

            return drawer.Element;
        }
    }
}