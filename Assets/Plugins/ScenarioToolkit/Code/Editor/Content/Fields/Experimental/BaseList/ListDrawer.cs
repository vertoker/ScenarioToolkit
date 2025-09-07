using System;
using Scenario.Editor.Content.Fields.Experimental.Lists;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Scenario.Editor.Content.Fields.Experimental.BaseList
{
    public class ListDrawer
    {
        public readonly PropertyList List;
        public readonly VisualElement Element;
        
        private readonly BaseListScriptableObject listScriptableObject;
        private readonly SerializedObject serializedObject;
        private readonly SerializedProperty serializedProperty;

        public ListDrawer(Type scriptableType)
        {
            listScriptableObject = (BaseListScriptableObject)ScriptableObject.CreateInstance(scriptableType);
            serializedObject = new SerializedObject(listScriptableObject);
            serializedProperty = serializedObject.FindProperty(listScriptableObject.GetListName());
            List = new PropertyList(serializedProperty);
            var scroll = new Vector2();
            var lastHash = serializedProperty.contentHash;
            Element = new IMGUIContainer(() =>
            {
                scroll = GUILayout.BeginScrollView(scroll);
                EditorGUILayout.PropertyField(serializedProperty);
                GUILayout.EndScrollView();
                serializedObject.ApplyModifiedProperties();
                var newHash = serializedProperty.contentHash;
                if (lastHash != newHash)
                    List.ExternalItemsChanged();
            });
            
        }
        
        ~ListDrawer()
        {
            Object.DestroyImmediate(listScriptableObject);
            serializedProperty.Dispose();
            serializedObject.Dispose();
        }
    }
}