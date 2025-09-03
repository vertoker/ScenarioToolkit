using System;
using System.Collections.Generic;
using VRF.DataSources.Interfaces;

namespace VRF.DataSources.LocalCache
{
    /// <summary>
    /// Источник данных изнутри приложения, нужен для передачи данных между контекстами
    /// </summary>
    public class LocalCacheDataSource : IDataSource, IDataLoader, IDataSaver
    {
        public LocalCacheDataSourceSettings Settings { get; }
        public DataSourceType GetSelfType() => DataSourceType.LocalCache;
        
        private readonly Dictionary<Type, object> models;

        public LocalCacheDataSource(LocalCacheDataSourceSettings settings)
        {
            Settings = settings;
            models = new Dictionary<Type, object>();
        }

        public bool Add<TModel>(TModel model)
        {
            Add(model.GetType(), model);
            return true;
        }
        public bool Add(Type type, object model)
        {
            models[type] = model;
            Settings.Debug.TryLogModified($"Add model <b>{type}</b>");
            return true;
        }
        
        public bool Remove<TModel>(TModel model)
        {
            return Remove(model.GetType());
        }
        public bool Remove<TModel>()
        {
            return Remove(typeof(TModel));
        }
        public bool Remove(Type type)
        {
            Settings.Debug.TryLogModified($"Remove model <b>{type}</b>");
            return models.Remove(type);
        }
        public bool Remove<TModel>(out TModel model)
        {
            var result = Remove(typeof(TModel), out var modelObj);
            model = (TModel)modelObj;
            return result;
        }
        public bool Remove(Type type, out object objModel)
        {
            Settings.Debug.TryLogModified($"Remove model <b>{type}</b>");
            return models.Remove(type, out objModel);
        }
        
        public bool Save<TData>(TData data)
        {
            Add(data);
            return true;
        }
        public TData Load<TData>() where TData : class
        {
            foreach (var model in models)
            {
                if (model.Key == typeof(TData))
                {
                    Settings.Debug.TryLogLoaded($"Load model <b>{typeof(TData).Name}</b>");
                    return (TData)model.Value;
                }
            }
            
            Settings.Debug.TryLogLoadFailed($"Can't find any model <b>{typeof(TData).Name}</b>");
            return default;
        }
    }
}