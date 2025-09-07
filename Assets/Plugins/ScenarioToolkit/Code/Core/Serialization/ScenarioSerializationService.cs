using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using ScenarioToolkit.Core.Serialization.Converters;
using ScenarioToolkit.Core.Serialization.Converters.Base;
using ScenarioToolkit.Shared;
using UnityEngine;
using Zenject;

namespace ScenarioToolkit.Core.Serialization
{
    /// <summary>
    /// Основополагающий сервис для работы сериализации сценариев.
    /// Нужен для связи Unity объектов и сценария
    /// </summary>
    public class ScenarioSerializationService
    {
        private JsonConverter[] converters;
        private IScenarioJsonConverter[] scenarioConverters;
        
        public JsonSerializerSettings Settings { get; private set; }
        public JsonSerializer Serializer { get; private set; }

        public IReadOnlyList<JsonConverter> Converters => converters;
        public IReadOnlyList<IScenarioJsonConverter> ScenarioConverters => scenarioConverters;
        
        [Inject]
        public ScenarioSerializationService(ScenarioSerializationSettings settings)
        {
            // Исправление бага для десериализации пустых HashSet и List, на Android IL2CPP
            // они сериализуются не пустыми, а null и поэтому всё ломается при загрузке сценариев
#if ENABLE_IL2CPP
            AotHelper.EnsureList<int>();
            // HashSet of Links build deserialization
            AotHelper.Ensure(() => _ = new HashSet<int>(Array.Empty<int>()));
#endif
            Init(settings);
        }
        public ScenarioSerializationService()
        {
            Init();
        }

        /// <summary>
        /// Инициализация, вынесенная в отдельный метод
        /// Механизм - полное дерьмо, но он нужен для перезагрузки в редакторе
        /// </summary>
        public void Init()
        {
            Init(new ScenarioSerializationSettings());
        }
        public void Init(ScenarioSerializationSettings settings)
        {
            converters = GetConverters();
            scenarioConverters = converters.Select(c => (IScenarioJsonConverter)c).ToArray();
            
            settings ??= new ScenarioSerializationSettings();
            foreach (var scenarioConverter in scenarioConverters)
                scenarioConverter.Set(settings);
            
            Settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.Objects,
                Converters = converters, Error = (sender, args) =>
                {
                    if (settings.LogErrors && Application.platform == RuntimePlatform.Android)
                    {
                        // Рассчитано на отладку в Android ADB консоли, так читается более удобно
                        Debug.LogError($"sender: {sender}");
                        Debug.LogWarning("--------------------------------------------");
                        Debug.LogError($"object: {args.CurrentObject}");
                        Debug.LogWarning("--------------------------------------------");
                        Debug.LogError($"error: {args.ErrorContext.Error}, ");
                        Debug.LogWarning("--------------------------------------------");
                        Debug.LogError($"member: {args.ErrorContext.Member}, ");
                        Debug.LogWarning("--------------------------------------------");
                        Debug.LogError($"Path: {args.ErrorContext.Path}, ");
                        Debug.LogWarning("--------------------------------------------");
                        Debug.LogError($"OriginalObject: {args.ErrorContext.OriginalObject}, ");
                        Debug.Log("***************************************************");
                    }
                }
            };
            Serializer = JsonSerializer.Create(Settings);
        }
        
        public bool IsValidConverters()
        {
            var converterGO = (ScenarioGameObjectConverter)Settings.Converters
                .First(c => c.GetType() == typeof(ScenarioGameObjectConverter));
            var validGO = converterGO.Provider;

            return validGO;
        }
        
        public T Deserialize<T>(string json) => JsonConvert.DeserializeObject<T>(json, Settings);
        public object Deserialize(string json, Type type) => JsonConvert.DeserializeObject(json, type, Settings);

        public T Deserialize<T>(StringReader stringReader)
        {
            var jsonSerializer = JsonSerializer.CreateDefault(Settings);
            using var reader = new JsonTextReader(stringReader);
            return (T)jsonSerializer.Deserialize(reader, typeof(T));
        }
        public object Deserialize(StringReader stringReader, Type type)
        {
            var jsonSerializer = JsonSerializer.CreateDefault(Settings);
            using var reader = new JsonTextReader(stringReader);
            return jsonSerializer.Deserialize(reader, type);
        }
        public T Deserialize<T>(JsonTextReader jsonTextReader)
        {
            var jsonSerializer = JsonSerializer.CreateDefault(Settings);
            return (T)jsonSerializer.Deserialize(jsonTextReader, typeof(T));
        }
        public object Deserialize(JsonTextReader jsonTextReader, Type type)
        {
            var jsonSerializer = JsonSerializer.CreateDefault(Settings);
            return jsonSerializer.Deserialize(jsonTextReader, type);
        }
        
        public string Serialize<T>(T data) => JsonConvert.SerializeObject(data, Settings);
        public string Serialize(object data, Type type) => JsonConvert.SerializeObject(data, type, Settings);
        
        public byte[] SerializeBytes<T>(T data) => SerializeBytes(data, typeof(T));
        public T DeserializeBytes<T>(byte[] bytes) => (T)DeserializeBytes(bytes, typeof(T));
        public byte[] SerializeBytes(object data, Type type)
        {
            using var stringWriter = new StringWriter();
            Serializer.Serialize(stringWriter, data, type);
            var json = stringWriter.ToString();
            var bytes = Encoding.UTF8.GetBytes(json);
            return bytes;
        }
        public object DeserializeBytes(byte[] bytes, Type type)
        {
            var json = Encoding.UTF8.GetString(bytes);
            var stringReader = new StringReader(json);
            var jsonTextReader = new JsonTextReader(stringReader);
            var data = Serializer.Deserialize(jsonTextReader, type);
            return data;
        }

        private static JsonConverter[] GetConverters()
        {
            var implementationTypes = Reflection.GetImplementations<IScenarioJsonConverter>();
            var converters = implementationTypes.Select(Activator.CreateInstance).Select(i => (JsonConverter)i);
            return converters.ToArray();
        }
    }
}