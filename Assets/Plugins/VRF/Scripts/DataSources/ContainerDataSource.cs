using System.Collections.Generic;
using UnityEngine;
using VRF.DataSources.Interfaces;

namespace VRF.DataSources
{
    /// <summary>
    /// Основной источник всех данных, является простейшим агрегатором схожих интерфейсов ISave/ILoad
    /// </summary>
    public class ContainerDataSource : IDataSaver, IDataLoader
    {
        /// <summary> Внутреннее хранилище для реализации интерфейсов </summary>
        private readonly struct DataSourceEntry
        {
            public readonly IDataLoader Loader;
            public readonly IDataSaver Saver;

            public DataSourceEntry(IDataSource source)
            {
                Loader = source as IDataLoader;
                Saver = source as IDataSaver;
            }

            /// <summary> Имеются ли интерфейсы загрузки/сохранения в источнике данных? </summary>
            public bool IsAnyDataSource() => Loader != null || Saver != null;
        }

        /// <summary> Настройки source </summary>
        public ContainerDataSourceSettings Settings { get; }

        /// <summary> Все занесённые интерфейсы для работы  </summary>
        private readonly Dictionary<DataSourceType, DataSourceEntry> dataSources = new();

        public ContainerDataSource(ContainerDataSourceSettings settings)
        {
            Settings = settings;
        }

        public void AddDataSources(params IDataSource[] newDataSources)
        {
            foreach (var dataSource in newDataSources)
                AddDataSource(dataSource);
        }
        /// <summary> Добавляет в контейнер реализацию ISave/ILoad </summary>
        public void AddDataSource(IDataSource dataSource)
        {
            var entry = new DataSourceEntry(dataSource);
            if (entry.IsAnyDataSource())
                dataSources.Add(dataSource.GetSelfType(), entry);
        }

        /// <summary>
        /// Расширенная имплементация интерфейса сохранения,
        /// напрямую можно указать приоритетный источник загрузки
        /// </summary>
        public bool Save<TData>(TData data, DataSourceType prioritySource)
        {
            var sourceTypes = Settings.GeneratePriorityList(prioritySource);
            return Save(data, sourceTypes);
        }

        /// <summary>
        /// Расширенная имплементация интерфейса загрузки,
        /// напрямую можно указать приоритетный источник загрузки
        /// </summary>
        public TData Load<TData>(DataSourceType prioritySource) where TData : class
        {
            var sourceTypes = Settings.GeneratePriorityList(prioritySource);
            return Load<TData>(sourceTypes);
        }

        /// <summary>
        /// Стандартная имплементация интерфейса сохранения,
        /// использует источники в стандартном порядке
        /// </summary>
        public bool Save<TData>(TData data)
            => Save(data, Settings.GetDefault());

        /// <summary>
        /// Стандартная имплементация интерфейса загрузки,
        /// использует источники в стандартном порядке
        /// </summary>
        public TData Load<TData>() where TData : class
            => Load<TData>(Settings.GetDefault());

        /// <summary>
        /// Общая имплементация сохранения со списком источников
        /// </summary>
        public bool Save<TData>(TData data, IEnumerable<DataSourceType> sourceTypes)
        {
            foreach (var sourceType in sourceTypes)
            {
                if (!dataSources.TryGetValue(sourceType, out var entry))
                {
                    Settings.Debug.TryLogSaveFailed($"Can't find <b>{sourceType}</b>");
                    continue;
                }

                if (entry.Saver == null)
                {
                    Settings.Debug.TryLogSaveFailed($"Can't find saver in dataSource <b>{sourceType}</b>");
                    continue;
                }
                if (!entry.Saver.Save(data))
                {
                    Settings.Debug.TryLogSaveFailed($"Can't save model <b>{typeof(TData).Name}</b> " +
                                                    $"via <b>{sourceType}</b>");
                    continue;
                }
                
                Settings.Debug.TryLogSaved($"Save model <b>{typeof(TData).Name}</b> " +
                                           $"via dataSource <b>{sourceType}</b>");
                return true;
            }

            Settings.Debug.TryLogSaveFailed($"Can't save model <b>{typeof(TData).Name}</b>");
            return false;
        }

        /// <summary>
        /// Общая имплементация загрузки со списком источников
        /// </summary>
        public TData Load<TData>(IEnumerable<DataSourceType> sourceTypes) where TData : class
        {
            foreach (var sourceType in sourceTypes)
            {
                if (!dataSources.TryGetValue(sourceType, out var entry))
                {
                    Settings.Debug.TryLogLoadFailed($"Can't find dataSource <b>{sourceType}</b>");
                    continue;
                }

                var data = entry.Loader?.Load<TData>();
                if (data == null)
                {
                    Settings.Debug.TryLogLoadFailed($"Can't load model <b>{typeof(TData).Name}</b> " +
                                                    $"via <b>{sourceType}</b>");
                    continue;
                }

                Settings.Debug.TryLogLoaded($"Load model <b>{typeof(TData).Name}</b> " +
                                            $"via <b>{sourceType}</b>");
                return data;
            }

            Settings.Debug.TryLogLoadFailed($"Can't load model <b>{typeof(TData).Name}</b>");
            return default;
        }
    }
}