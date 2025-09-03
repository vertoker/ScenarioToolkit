using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using VRF.DataSources.Interfaces;

namespace VRF.DataSources.Config
{
    /// <summary>
    /// Источник данных из текстовых файлов, имеет также возмножность сохранения, использует Newtonsoft Json
    /// </summary>
    public class ConfigDataSource : IDataSource, IDataSaver, IDataLoader
    {
        public ConfigDataSourceSettings Settings { get; }
        public DataSourceType GetSelfType() => DataSourceType.ConfigParser;

        public ConfigDataSource(ConfigDataSourceSettings settings)
        {
            Settings = settings;
        }

        public bool Save<TData>(TData data)
        {
            var text = JsonConvert.SerializeObject(data, Formatting.Indented);
            var filePath = SaveInternal<TData>();
            File.WriteAllText(filePath, text);
            Settings.Debug.TryLogSaved($"Save model <b>{typeof(TData).Name}</b>");
            return true;
        }
        public TData Load<TData>() where TData : class
        {
            var filePath = GetFilePath<TData>();
            var fileExists = File.Exists(filePath);
            var data = fileExists ? LoadInternal<TData>(filePath) : default;
            
            if (data == null)
                Settings.Debug.TryLogLoadFailed($"Can't load model <b>{typeof(TData).Name}</b>");
            Settings.Debug.TryLogLoaded($"Load model <b>{typeof(TData).Name}</b>");
            
            return data;
        }

        private static TData LoadInternal<TData>(string filePath)
        {
            var text = File.ReadAllText(filePath);
            var data = JsonConvert.DeserializeObject<TData>(text);
            return data;
        }

        private TData CreateAndSave<TData>() where TData : new()
        {
            var dataDefault = new TData();
            Save(dataDefault);
            return dataDefault;
        }

        private string SaveInternal<TData>()
        {
            var directory = GetDirectory();
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            var fileName = GetFileName<TData>();
            var filePath = Path.Combine(directory, fileName);
            return filePath;
        }


        private string GetFilePath<TData>()
        {
            var directory = GetDirectory();
            var fileName = GetFileName<TData>();
            return Path.Combine(directory, fileName);
        }

        // Для имени файла используется имя модели
        private string GetFileName<TData>() => $"{typeof(TData).Name}.{Settings.ConfigExtension}";

        // Именно этот метод имплементирует все пути из ConfigFolder
        private string GetDirectory()
        {
            return Settings.Folder switch
            {
                ConfigFolder.StreamingAssets => Application.streamingAssetsPath,
                ConfigFolder.StreamingAssetsConfigs => Path.Combine(Application.streamingAssetsPath, "Configs"),
                ConfigFolder.PersistentData => Path.Combine(Application.persistentDataPath),
                _ => throw new ArgumentOutOfRangeException(nameof(Settings.Folder), Settings.Folder, null)
            };
        }
    }
}