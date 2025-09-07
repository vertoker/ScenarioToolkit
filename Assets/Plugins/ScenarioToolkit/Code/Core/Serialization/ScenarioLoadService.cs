using System;
using System.IO;
using Newtonsoft.Json;
using Scenario.Core.Model.Interfaces;
using Scenario.Utilities;
using UnityEngine;
using Zenject;

namespace Scenario.Core.Serialization
{
    /// <summary>
    /// Сервис сохранения/загрузки JSON сценария для runtime модели
    /// </summary>
    public class ScenarioLoadService
    {
        public ScenarioSerializationService SerializationService { get; }
        
        public const string SerializationHeader = "application/vnd.unity.graphview.elements ";

        [Inject]
        public ScenarioLoadService(ScenarioSerializationService serializationService)
        {
            SerializationService = serializationService;
        }
        public ScenarioLoadService(ScenarioSerializationSettings settings)
        {
            SerializationService = new ScenarioSerializationService(settings);
        }
        public ScenarioLoadService()
        {
            SerializationService = new ScenarioSerializationService();
        }

        public IScenarioModel LoadModelFromPath(string path, bool convert = false)
        {
            var json = File.ReadAllText(path);
            return LoadModelFromJson(json, convert);
        }
        public IScenarioModel LoadModelFromJson(string json, bool convert = false)
        {
            // Специальный режим загрузки нужен для того, чтобы грузить модель если
            // достоверно неизвестно, что она последней версии
            
            // For compability with V0 models and copy/paste functions
            //if (json.StartsWith(SerializationHeader)) // remove header if it has
            //    json = json[SerializationHeader.Length..]; // can be optimized

            var stringReader = new StringReader(json);
            if (json.StartsWith(SerializationHeader))
                for (var i = 0; i < SerializationHeader.Length; i++)
                    stringReader.Read();
            
            if (convert)
            {
                var type = GetModelType(json);
                
                // Если это не последняя модель
                if (!typeof(IScenarioModel).IsAssignableFrom(type))
                {
                    var model = SerializationService.Deserialize(json, type);
                    return ScenarioCompatibilityService.UpdateModel(model);
                }
            }
            
            return (IScenarioModel)SerializationService.Deserialize(stringReader, IScenarioModel.GetModelType);
        }
        public bool TryLoadModelFromPath(string path, bool convert, out IScenarioModel model)
        {
            if (!File.Exists(path))
            {
                model = default;
                return false;
            }
            
            var json = File.ReadAllText(path);
            return TryLoadModelFromJson(json, convert, out model);
        }
        public bool TryLoadModelFromJson(string json, bool convert, out IScenarioModel model)
        {
            try
            {
                model = LoadModelFromJson(json, convert);
                return true;
            }
            catch (JsonReaderException)
            {
                model = IScenarioModel.CreateNew();
                return false;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                model = IScenarioModel.CreateNew();
                return false;
            }
        }
        
        public TModel LoadFromPath<TModel>(string path)
        {
            var json = File.ReadAllText(path);
            return LoadFromJson<TModel>(json);
        }
        public TModel LoadFromJson<TModel>(string json)
        {
            // For compability with V0 models and copy/paste functions
            //if (json.StartsWith(SerializationHeader)) // remove header if it has
            //    json = json[SerializationHeader.Length..]; // can be optimized

            var stringReader = new StringReader(json);
            if (json.StartsWith(SerializationHeader))
                for (var i = 0; i < SerializationHeader.Length; i++)
                    stringReader.Read();
            
            return SerializationService.Deserialize<TModel>(stringReader);
        }
        public bool TryLoadFromPath<TModel>(string path, out TModel model)
        {
            if (!File.Exists(path))
            {
                model = default;
                return false;
            }
            
            var json = File.ReadAllText(path);
            return TryLoadFromJson(json, out model);
        }
        public bool TryLoadFromJson<TModel>(string json, out TModel model)
        {
            try
            {
                model = LoadFromJson<TModel>(json);
                return true;
            }
            catch (JsonReaderException)
            {
                model = default;
                return false;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                model = default;
                return false;
            }
        }
        
        /// <summary>
        /// Находит тип runtime модели в загруженном json
        /// </summary>
        public static Type GetModelType(string modelJson)
        {
            if (modelJson.StartsWith(SerializationHeader)) // remove header if it has
                modelJson = modelJson[SerializationHeader.Length..]; // can be optimized
            
            using var stringReader = new StringReader(modelJson);
            using var jsonReader = new JsonTextReader(stringReader);

            if (jsonReader.TokenType == JsonToken.None)
                jsonReader.Read();
            if (jsonReader.TokenType == JsonToken.StartObject)
                jsonReader.Read();
            if (jsonReader.TokenType == JsonToken.PropertyName && (string)jsonReader.Value == "$type")
                jsonReader.Read();
            
            // TokenType == Value
            if (jsonReader.Value is not string typeName)
                throw new ArgumentException("Not a string", nameof(jsonReader));
            if (!ScenarioTypeParser.TryDeserialize(typeName, out var type))
                throw new ArgumentException("Not a type", nameof(jsonReader));
            
            return type;
        }

        public static bool CheckValidJson(string json)
        {
            if (json.StartsWith(SerializationHeader))
                json = json[SerializationHeader.Length..];
            
            using var stringReader = new StringReader(json);
            using var jsonReader = new JsonTextReader(stringReader);

            try
            {
                jsonReader.Read();
                
                return true;
            }
            catch (JsonReaderException)
            {
                return false;
            }

            throw new Exception();
        }
    }
}