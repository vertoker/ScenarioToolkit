using System;
using ModestTree;
using Newtonsoft.Json;
using Scenario.Utilities;
using ScenarioToolkit.Shared;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
    /// <summary>
    /// Объект-адрес, который ссылается к конкретному компоненту конкретного типа
    /// </summary>
    public readonly struct IndexedComponent
    {
        [JsonProperty] public readonly GameObject GameObject;
        [JsonProperty] public readonly Type Type;
        [JsonProperty] public readonly int Index;

        public IndexedComponent(Component source, bool log = true)
        {
            if (!source)
            {
                if (log)
                    Debug.LogWarning($"[W] Can't write component, component is null");
                GameObject = null;
                Type = null;
                Index = -1;
                return;
            }
            
            GameObject = source.gameObject;
            Type = source.GetType();
            
            if (!GameObject)
            {
                if (log)
                    Debug.LogWarning($"[W] Can't write component, GameObject is not exist");
                Index = -1;
                return;
            }
            
            var components = GameObject.GetComponents(Type);
            
            if (components.Length == 0)
            {
                if (log)
                    Debug.LogWarning($"[W] Can't write component, components of the same type doesn't have in GameObject");
                Index = -1;
                return;
            }
            
            Index = components.IndexOf(source);
            
            if (Index == -1 && log)
                Debug.LogWarning($"[W] Can't write component, can't find index of source");
        }

        public Component GetComponent(bool log = true)
        {
            if (Type == TypesReflection.SerializationNullType)
            {
                if (log)
                    Debug.LogWarning($"[R] Can't deserialize component (<b>type is null</b>)");
                return null;
            }

            if (!typeof(Component).IsAssignableFrom(Type))
            {
                if (log)
                    Debug.LogWarning($"[R] Can't deserialize component <b>{Type.Name}</b> " + 
                                     $"(<b>type is not Component</b>)");
                return null;
            }

            if (!GameObject)
            {
                if (log)
                    Debug.LogWarning($"[R] Can't deserialize component <b>{Type.Name}</b> " + 
                                     $"(<b>no {nameof(GameObject)}</b>)");
                return null;
            }

            var components = GameObject.GetComponents(Type);
                
            if (components.Length == 0)
            {
                if (log)
                    Debug.LogWarning($"[R] Can't deserialize component <b>{Type.Name}</b> " + 
                                     $"on <b>{GameObject.name}</b> (<b>doesn't have any {Type.Name} components</b>)");
                return null;
            }

            if (Index >= components.Length || Index < 0)
            {
                if (log) 
                    Debug.LogWarning($"[R] Can't deserialize component <b>{Type.Name}</b> " +
                                     $"on <b>{GameObject.name}</b> (<b>Index {Index} is out of bounds) " +
                                     $"(components count is {components.Length})</b>)");
                return null;
            }

            var component = components[Index];

            if (!component)
            {
                if (log) 
                    Debug.LogWarning($"[R] Can't deserialize component <b>{Type.Name}</b> " +
                                     $"on <b>{GameObject.name}</b> (<b>Component null or destroyed</b>)");
                return null;
            }
                
            return component;
        }
    }
}