using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace ScenarioToolkit.Editor.Content.Fields.Experimental.BaseList
{
    public class PropertyList : IList, IEnumerable<object>
    {
        public event Action<IEnumerable<object>> ItemsChanged; 
        
        private readonly SerializedProperty serializedProperty;
        
        public int Count => serializedProperty.arraySize;
        public bool IsSynchronized => false;
        public object SyncRoot => null!;
        public bool IsReadOnly => false;
        public bool IsFixedSize => false;
        
        public PropertyList(SerializedProperty property)
        {
            serializedProperty = property;
        }

        public void ExternalItemsChanged()
        {
            ItemsChanged?.Invoke(this);
        }

        public IEnumerator<object> GetEnumerator()
        {
            for (var i = 0; i < serializedProperty.arraySize; i++)
                yield return serializedProperty.GetArrayElementAtIndex(i).boxedValue;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int Add(object item)
        {
            var pos = serializedProperty.arraySize; 
            Insert(pos, item);
            ItemsChanged?.Invoke(this);
            return pos;
        }
        public void Remove(object item)
        {
            var index = IndexOf(item);
            if (index < 0)
                return;
            RemoveAt(index);
            ItemsChanged?.Invoke(this);
        }

        public void Clear()
        {
            serializedProperty.ClearArray();
            ItemsChanged?.Invoke(this);
        }
        public bool Contains(object item)
        {
            return IndexOf(item) >= 0;
        }

        public void CopyTo(Array array, int index)
        {
            for (var i = 0; i < serializedProperty.arraySize; i++)
                array.SetValue(this[i], index + i);
        }

        public int IndexOf(object item)
        {
            for (var i = 0; i < serializedProperty.arraySize; i++)
                if (serializedProperty.GetArrayElementAtIndex(i).boxedValue == item)
                    return i;
            return -1;
        }

        public void Insert(int index, object item)
        {
            serializedProperty.InsertArrayElementAtIndex(index);
            var prop = serializedProperty.GetArrayElementAtIndex(index);
            prop.boxedValue = item;
            ItemsChanged?.Invoke(this);
        }

        public void RemoveAt(int index)
        {
            serializedProperty.DeleteArrayElementAtIndex(index);
            ItemsChanged?.Invoke(this);
        }

        public object this[int index]
        {
            get => serializedProperty.GetArrayElementAtIndex(index).boxedValue;
            set => serializedProperty.GetArrayElementAtIndex(index).boxedValue = value;
        }
    }
}