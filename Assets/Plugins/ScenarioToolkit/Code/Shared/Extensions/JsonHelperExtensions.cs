using System;
using System.Globalization;
using Newtonsoft.Json;
using UnityEngine;

namespace ScenarioToolkit.Shared.Extensions
{
    // Inspired by https://github.com/applejag/Newtonsoft.Json-for-Unity.Converters/blob/master/Packages/Newtonsoft.Json-for-Unity.Converters/UnityConverters/Helpers/JsonHelperExtensions.cs
    
    public static class JsonHelperExtensions
    {
        public static void WritePropertyValue(this JsonWriter writer, string propertyName, float value)
        {
            writer.WritePropertyName(propertyName);
            writer.WriteValue(value);
        }
        public static void WritePropertyValue(this JsonWriter writer, string propertyName, int value)
        {
            writer.WritePropertyName(propertyName);
            writer.WriteValue(value);
        }
        public static void WritePropertyValue(this JsonWriter writer, string propertyName, byte value)
        {
            writer.WritePropertyName(propertyName);
            writer.WriteValue(value);
        }
        public static void WritePropertyValue(this JsonWriter writer, string propertyName, string value)
        {
            writer.WritePropertyName(propertyName);
            writer.WriteValue(value);
        }
        
        public static float? ReadAsFloat(this JsonReader reader)
        {
            // https://github.com/jilleJr/Newtonsoft.Json-for-Unity.Converters/issues/46

            var str = reader.ReadAsString();

            if (string.IsNullOrEmpty(str)) return null;
            if (float.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out var valueParsed))
                return valueParsed;
            
            return 0f;
        }
        public static byte? ReadAsByte(this JsonReader reader)
        {
            return checked((byte)(reader.ReadAsInt32() ?? 0));
        }

        public static bool AssertNotNull(this JsonReader reader, Type objectType, bool debug = true)
        {
            if (reader.TokenType != JsonToken.Null) return false;
            if (debug)
            {
                Debug.LogWarning($"[R] Can't serialize <b>{objectType.Name}</b> " +
                                 $"in JSON at path <b>{reader.Path}</b>");
            }
            return true;
        }
        public static bool AssertTokenIs(this JsonReader reader, JsonToken token, bool debug = true)
        {
            if (reader.TokenType == token) return false;
            if (debug)
            {
                Debug.LogWarning($"[R] Token in the reader must be <b>{token}</b>, " +
                                 $"but it is <b>{reader.TokenType}</b> " +
                                 $"(JSON path is <b>{reader.Path}</b>)");
            }
            return true;
        }
    }
}