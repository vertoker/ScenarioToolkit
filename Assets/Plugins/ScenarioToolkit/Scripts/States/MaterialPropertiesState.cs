using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Scenario.Core.Systems.States;
using UnityEngine;

namespace Scenario.States
{
    public class MaterialPropertiesState : IState
    {
        [JsonIgnore] public Dictionary<MaterialPropertyInfo, TextureData> DefaultTextureDatas = new();
        
        // (key), (default, current)
        // ReSharper disable once FieldCanBeMadeReadOnly.Global
        public Dictionary<MaterialPropertyInfo, (Color, Color)> Colors = new();
        public Dictionary<MaterialPropertyInfo, (float, float)> Floats = new();
        public Dictionary<MaterialPropertyInfo, (int, int)> Ints = new();
        public Dictionary<MaterialPropertyInfo, TextureData> Textures = new();
        public Dictionary<MaterialPropertyInfo, (Vector4, Vector4)> Vectors = new();

        public struct MaterialPropertyInfo : IEquatable<MaterialPropertyInfo>
        {
            public Renderer Renderer;
            public int MaterialIndex;
            public string PropertyName;

            public MaterialPropertyInfo(Renderer renderer, int materialIndex, string propertyName)
            {
                Renderer = renderer;
                MaterialIndex = materialIndex;
                PropertyName = propertyName;
            }

            public bool Equals(MaterialPropertyInfo other)
            {
                return Renderer.Equals(other.Renderer)
                       && MaterialIndex.Equals(other.MaterialIndex)
                       && PropertyName.Equals(other.PropertyName);
            }

            public override bool Equals(object obj)
            {
                return obj is MaterialPropertyInfo other && Equals(other);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(Renderer, MaterialIndex, PropertyName);
            }
        }
        public struct TextureData : IEquatable<TextureData>
        {
            public Texture Texture;

            public TextureData(Texture texture)
            {
                Texture = texture;
            }

            public bool Equals(TextureData other)
            {
                return Texture.Equals(other.Texture);
            }

            public override bool Equals(object obj)
            {
                return obj is TextureData other && Equals(other);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(Texture);
            }
        }

        public void Clear()
        {
            Colors.Clear();
            Floats.Clear();
            Ints.Clear();
            Textures.Clear();
            Vectors.Clear();
        }
    }
}