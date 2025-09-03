using System;
using System.Collections.Generic;
using UnityEngine;
using VRF.DataSources.Interfaces;

namespace VRF.DataSources.Scriptables
{
    /// <summary>
    /// Источник данных из Scriptable объектов, загружаются не сами объекты, а модели внутри них
    /// </summary>
    public class ScriptableDataSource : IDataSource, IDataLoader, IHandlerContainer<IScriptableModel>
    {
        private readonly Dictionary<Type, IScriptableModel> scriptables;
        
        public ScriptableDataSourceSettings Settings { get; }
        
        public DataSourceType GetSelfType() => DataSourceType.Scriptable;

        public ScriptableDataSource(ScriptableDataSourceSettings settings)
        {
            Settings = settings;
            scriptables = new Dictionary<Type, IScriptableModel>();
        }
        
        public void Add(IScriptableModel scriptableModel)
        {
            var type = scriptableModel.GetModelType();
            
            if (!scriptables.TryAdd(type, scriptableModel))
                Settings.Debug.LogWarning($"Can't add scriptable, same model type <b>{type}</b>, " +
                                          $"scriptable type <b>{scriptableModel.GetType()}</b>");
            
            Settings.Debug.TryLogModified($"Add scriptable <b>{scriptableModel.GetType()}</b> " +
                                          $"for model <b>{type}</b>");
        }
        public Action GetRemovePromise(IScriptableModel scriptableModel) => () => Remove(scriptableModel);
        public void Remove(IScriptableModel scriptableModel)
        {
            var type = scriptableModel.GetModelType();
            scriptables.Remove(type);
            Settings.Debug.TryLogModified($"Remove scriptable <b>{scriptableModel.GetType()}</b> " +
                                          $"for model <b>{type}</b>");
        }

        public TData Load<TData>() where TData : class
        {
            foreach (var scriptable in scriptables)
            {
                if (scriptable.Key == typeof(TData))
                {
                    var data = scriptable.Value.GetModelObject();

                    if (data == null)
                    {
                        Settings.Debug.TryLogLoadFailed($"Null model <b>{typeof(TData).Name}</b> " +
                                                        $"in <b>{scriptable.Value.GetType().Name}</b>");
                        return default;
                    }

                    Settings.Debug.TryLogLoaded($"Load model <b>{typeof(TData).Name}</b> " +
                                                $"via <b>{scriptable.Value.GetType().Name}</b>");
                    return (TData)data;
                }
            }
            
            Settings.Debug.TryLogLoadFailed($"Can't find any scriptable for model <b>{typeof(TData).Name}</b>");
            return default;
        }
    }
}