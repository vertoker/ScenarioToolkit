using System;
using UnityEngine;

namespace ScenarioToolkit.Core.Serialization
{
    [Serializable]
    public class ScenarioSerializationSettings
    {
        [SerializeField] private bool logWarnings;
        [SerializeField] private bool logErrors;
        [SerializeField] private bool logMessages;

        public bool LogWarnings => logWarnings;
        public bool LogErrors => logErrors;
        public bool LogMessages => logMessages;
        
        public ScenarioSerializationSettings()
        {
            logWarnings = false;
            logErrors = true;
            logMessages = true;
        }
        public ScenarioSerializationSettings(bool logWarnings = false, bool logErrors = true, bool logMessages = true)
        {
            this.logWarnings = logWarnings;
            this.logErrors = logErrors;
            this.logMessages = logMessages;
        }
    }
}