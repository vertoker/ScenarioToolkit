using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using VRF.Utilities.Attributes;

namespace VRF.Editor.MethodTools.Services
{
    public class StaticMethodContainer
    {
        public readonly struct Entry
        {
            public readonly MethodInfo Method;
            public readonly ToolMethodAttribute Attribute;

            public Entry(MethodInfo method, ToolMethodAttribute attribute)
            {
                Method = method;
                Attribute = attribute;
            }
        }
        
        private readonly List<Entry> _units = new();
        
        public IReadOnlyList<Entry> Units => _units;
        public IList UnitsNonGeneric => _units;

        private readonly int _stepCountPerUpdate;
        private readonly Action<MethodInfo, ToolMethodAttribute> _onAdd;
        private readonly Action _onClear;

        private bool _enabled;
        private int _typesCounter;
        private IReadOnlyList<Type> _types;

        public StaticMethodContainer(int stepCountPerUpdate, Action<MethodInfo, ToolMethodAttribute> onAdd, Action onClear)
        {
            _stepCountPerUpdate = stepCountPerUpdate;
            _onAdd = onAdd;
            _onClear = onClear;
        }
        
        public void Start()
        {
            _typesCounter = 0;
            _types = TypesCacheEditor.GetLazyTypes();
            //Debug.Log(_types.Count);
            _enabled = true;
        }
        public void Stop()
        {
            _enabled = false;
            _units.Clear();
            _onClear.Invoke();
        }
        
        public void Update()
        {
            if (!_enabled) return;
            var counterEnd = Mathf.Min(_typesCounter + _stepCountPerUpdate, _types.Count);
            
            while (_typesCounter < counterEnd)
            {
                ParseStaticUnits(_types[_typesCounter]);
                _typesCounter++;
            }
        }

        private void ParseStaticUnits(Type type)
        {
            // Test for a class
            if (type.IsClass == false) return;

            // Check each method for the attribute.
            foreach (var method in type.GetRuntimeMethods())
            {
                // Make sure the method is static
                if (method.IsStatic == false) continue;

                // Test for presence of the attribute
                var attribute = method.GetCustomAttribute<ToolMethodAttribute>();
                if (attribute == null) continue;

                //Debug.Log(method.Name);
                var entry = new Entry(method, attribute);
                _units.Add(entry);
                _onAdd.Invoke(entry.Method, entry.Attribute);
            }
        }
    }
}