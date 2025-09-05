using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

// Скопировано отсюда
// https://gist.github.com/Thundernerd/5085ec29819b2960f5ff2ee32ad57cbb

// Сделать InternalBridge нельзя, так как AssemblyDefinitionReference требует расширения именно UnityEditor.CoreModule
// Но для доступа к нему нужен AssemblyDefinition, которого в Unity просто нет

namespace Scenario.Editor.Utilities
{ 
    // ReSharper disable once CheckNamespace
    // ReSharper disable once InconsistentNaming
    public class _EditorWindow
    {
        private readonly EditorWindow instance;
        private readonly Type type;

        public _EditorWindow(EditorWindow instance)
        {
            this.instance = instance;
            type = instance.GetType();
        }

        public object m_Parent
        {
            get
            {
                var field = type.GetField(nameof(m_Parent), BindingFlags.Instance | BindingFlags.NonPublic);
                return field.GetValue(instance);
            }
        }
    }
    // ReSharper disable once CheckNamespace
    // ReSharper disable once InconsistentNaming
    public class _DockArea
    {
        private readonly object instance;
        private readonly Type type;

        public _DockArea(object instance)
        {
            this.instance = instance;
            type = instance.GetType();
        }

        public object window
        {
            get
            {
                var property = type.GetProperty(nameof(window), BindingFlags.Instance | BindingFlags.Public);
                return property.GetValue(instance, null);
            }
        }
        public object s_OriginalDragSource
        {
            set
            {
                var field = type.GetField(nameof(s_OriginalDragSource), BindingFlags.Static | BindingFlags.NonPublic);
                field.SetValue(null, value);
            }
        }
    }

    // ReSharper disable once CheckNamespace
    // ReSharper disable once InconsistentNaming
    public class _ContainerWindow
    {
        private readonly object instance;
        private readonly Type type;

        public _ContainerWindow(object instance)
        {
            this.instance = instance;
            type = instance.GetType();
        }
        
        public object rootSplitView
        {
            get
            {
                var property = type.GetProperty(nameof(rootSplitView), BindingFlags.Instance | BindingFlags.Public);
                return property.GetValue(instance, null);
            }
        }
    }

    // ReSharper disable once CheckNamespace
    // ReSharper disable once InconsistentNaming
    public class _SplitView
    {
        private readonly object instance;
        private readonly Type type;

        public _SplitView(object instance)
        {
            this.instance = instance;
            type = instance.GetType();
        }

        public object DragOver(EditorWindow child, Vector2 screenPoint)
        {
            var method = type.GetMethod(nameof(DragOver), BindingFlags.Instance | BindingFlags.Public);
            return method.Invoke(instance, new object[] { child, screenPoint });
        }
        public object PerformDrop(EditorWindow child, object dropInfo, Vector2 screenPoint)
        {
            //child.
            var method = type.GetMethod(nameof(PerformDrop), BindingFlags.Instance | BindingFlags.Public);
            return method.Invoke(instance, new[] { child, dropInfo, screenPoint });
        }
    }
}