using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using ScenarioToolkit.Core.Systems.States;
using UnityEngine;

namespace ScenarioToolkit.Library.States
{
    public class MaterialState : IState
    {
        // (key), (default, current)
        // ReSharper disable once FieldCanBeMadeReadOnly.Global
        [JsonIgnore] public Dictionary<(Renderer, int), Data> DefaultMaterials = new();
        public Dictionary<(Renderer, int), Data> Materials = new();
        
        public struct Data : IEquatable<Data>
        {
            // ReSharper disable once FieldCanBeMadeReadOnly.Global
            public Material Material;

            public Data(Material material)
            {
                Material = material;
            }

            public bool Equals(Data other)
            {
                return Equals(Material, other.Material);
            }
            public override bool Equals(object obj)
            {
                return obj is Data other && Equals(other);
            }
            public override int GetHashCode()
            {
                return Material ? Material.GetHashCode() : 0;
            }
        }

        public void Clear()
        {
            DefaultMaterials.Clear();
            Materials.Clear();
        }
    }
}