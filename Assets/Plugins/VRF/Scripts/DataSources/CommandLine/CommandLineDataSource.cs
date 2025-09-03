using System;
using System.Collections.Generic;
using UnityEngine;
using VRF.DataSources.Interfaces;
using VRF.DataSources.Scriptables;

namespace VRF.DataSources.CommandLine
{
    /// <summary>
    /// Источник данных из командной строки, использует для парсинга моделей кастомные парсеры
    /// </summary>
    public class CommandLineDataSource : IDataSource, IDataLoader, IHandlerContainer<IModelCmdParser>
    {
        public CommandLineParser Parser { get; }
        public CommandLineDataSourceSettings Settings { get; }
        public DataSourceType GetSelfType() => DataSourceType.CommandLine;

        private readonly Dictionary<Type, object> models;
        private readonly bool commandArgsIsEditor;
        
        public CommandLineDataSource(CommandLineDataSourceSettings settings)
        {
            Settings = settings;
            commandArgsIsEditor = Application.isEditor && settings.CommandLineConfig;
            Parser = commandArgsIsEditor
                ? new CommandLineParser(settings.CommandLineConfig.GetArgs(), settings.Debug.OnLoaded) 
                : new CommandLineParser(settings.Debug.OnLoaded);
            
            //if (Parser.IsEmpty())
            //    Debug.LogWarning($"Empty command line parameters, editor=<b>{nameof(Application.isEditor)}</b>");
            
            models = new Dictionary<Type, object>();
        }

        public void Add(IModelCmdParser modelParser)
        {
            var type = modelParser.GetModelType();
            if (models.ContainsKey(type)) return;
            
            // Парсинг модели ВСЕГДА происходит при добавлении, стоит учитывать

            if (!modelParser.CanParseModel(Parser))
            {
                Settings.Debug.TryLogLoadFailed($"Can't parse model type <b>{type}</b>, " +
                                                $"parser type <b>{modelParser.GetType()}</b>");
                return;
            }
            var model = modelParser.ParseModel(Parser);
            
            if (!models.TryAdd(type, model))
                Settings.Debug.LogWarning($"Can't add parser, same model type <b>{type}</b>, " +
                                          $"parser type <b>{modelParser.GetType()}</b>");
            
            Settings.Debug.TryLogModified($"Add parser <b>{modelParser.GetType()}</b> for model <b>{type}</b>");
        }
        public Action GetRemovePromise(IModelCmdParser modelParser) => () => Remove(modelParser);
        public void Remove(IModelCmdParser modelParser)
        {
            var type = modelParser.GetModelType();
            models.Remove(type);
            
            Settings.Debug.TryLogModified($"Remove parser <b>{modelParser.GetType()}</b> " +
                                          $"for model <b>{type}</b>");
        }
        
        public TData Load<TData>() where TData : class
        {
            if (Parser.IsEmpty())
            {
                Settings.Debug.TryLogLoadFailed($"Command args is empty, can't create any model, source: " +
                                                $"<b>{(commandArgsIsEditor ? "editor" : "runtime")}</b>");
                return default;
            }
            
            foreach (var model in models)
            {
                if (model.Key == typeof(TData))
                {
                    var data = (TData)model.Value;
                    Settings.Debug.TryLogLoaded($"Load model <b>{typeof(TData).Name}</b>");
                    return data;
                }
            }
            
            Settings.Debug.TryLogLoadFailed($"Can't find any parser for model <b>{typeof(TData).Name}</b>");
            return default;
        }
    }
}