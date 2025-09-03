using System;
using NaughtyAttributes;
using UnityEngine;
using VRF.DataSources.CommandLine.Scriptables;
using VRF.DataSources.Model;

namespace VRF.DataSources.CommandLine
{
    [Serializable]
    public class CommandLineDataSourceSettings
    {
        [SerializeField] private BaseCommandLineConfig commandLineConfig;
        
        [SerializeField] private CommandLineDebugDataSourceSettings debug = new();

        public BaseCommandLineConfig CommandLineConfig => commandLineConfig;
        public CommandLineDebugDataSourceSettings Debug => debug;

        public void Set(BaseCommandLineConfig newCommandLineConfig) 
            => commandLineConfig = newCommandLineConfig;
    }
}