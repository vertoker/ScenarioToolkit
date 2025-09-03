using System;
using NaughtyAttributes;
using UnityEngine;

namespace VRF.DataSources.Model
{
    /// <summary>
    /// Универсальные настройки для логирования действий, каждый source реализует по своему
    /// </summary>
    [Serializable]
    public class BaseDebugDataSourceSettings<TDataSource>
    {
        [SerializeField] private bool onSaved = true;
        [SerializeField] private bool onLoaded = true;
        [SerializeField] private bool onSaveFailed = false;
        [SerializeField] private bool onLoadFailed = false;
        [SerializeField] private bool onModified = false;
        
        public bool OnSaved => onSaved;
        public bool OnLoaded => onLoaded;
        public bool OnSaveFailed => onSaveFailed;
        public bool OnLoadFailed => onLoadFailed;
        public bool OnModified => onModified;

        public void TryLogLoaded(string message)
        {
            if (!OnLoaded) return;
            Log(message);
        }
        public void TryLogLoadFailed(string message)
        {
            if (!OnLoadFailed) return;
            LogWarning(message);
        }
        public void TryLogSaved(string message)
        {
            if (!OnSaved) return;
            Log(message);
        }
        public void TryLogSaveFailed(string message)
        {
            if (!OnSaveFailed) return;
            LogWarning(message);
        }
        public void TryLogModified(string message)
        {
            if (!OnModified) return;
            Log(message);
        }
        public void Log(string message)
        {
            Debug.Log($"{message} using <b>{typeof(TDataSource).Name}</b>");
        }
        public void LogWarning(string message)
        {
            Debug.LogWarning($"{message} using <b>{typeof(TDataSource).Name}</b>");
        }
        public void LogError(string message)
        {
            Debug.LogError($"{message} using <b>{typeof(TDataSource).Name}</b>");
        }
    }
}