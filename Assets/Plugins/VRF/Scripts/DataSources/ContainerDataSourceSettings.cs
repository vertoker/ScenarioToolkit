using System;
using System.Collections.Generic;
using UnityEngine;
using VRF.DataSources.Model;

namespace VRF.DataSources
{
    /// <summary> Настройки для ContainerDataSource </summary>
    [Serializable]
    public class ContainerDataSourceSettings
    {
        [SerializeField] private DataSourceType[] defaultEditor = DataSourceStatic.DefaultEditor;
        [SerializeField] private DataSourceType[] defaultRuntime = DataSourceStatic.DefaultRuntime;
        [SerializeField] private ContainerDebugDataSourceSettings debug = new();

        /// <summary> Исходный список источников данных </summary>
        public IReadOnlyList<DataSourceType> GetDefaultEditor() => defaultEditor;
        public IReadOnlyList<DataSourceType> GetDefaultRuntime() => defaultRuntime;
        public IReadOnlyList<DataSourceType> GetDefault() => DataSourceStatic.GetSources(defaultEditor, defaultRuntime);
        
        /// <summary> Настройки логирования Source </summary>
        public ContainerDebugDataSourceSettings Debug => debug;

        /// <summary>
        /// Создаёт очередь источников на основе стандартного списка
        /// и ставит в начало приоритетный
        /// </summary>
        /// <param name="primaryPriority">Приоритетный источник данных</param>
        /// <returns>Очередь источников с приоритетным в начале</returns>
        public DataSourceType[] GeneratePriorityList(DataSourceType primaryPriority)
        {
            var order = GetDefault();
            var length = order.Count;
            
            var sourcesPriority = new DataSourceType[length];
            for (var i = 0; i < length; i++)
                sourcesPriority[i] = order[i];

            var index = Array.IndexOf(sourcesPriority, primaryPriority);
            if (index != -1) 
                (sourcesPriority[0], sourcesPriority[index]) =
                    (sourcesPriority[index], sourcesPriority[0]);

            //Debug.Log(string.Join('-', sourcesPriority));
            return sourcesPriority;
        }
    }
}