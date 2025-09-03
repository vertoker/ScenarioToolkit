using System;
using UnityEngine;
using VRF.DataSources.Model;

namespace VRF.DataSources.Config
{
    [Serializable]
    public class ConfigDataSourceSettings
    {
        [SerializeField] private ConfigFolder folder = ConfigFolder.StreamingAssetsConfigs;
        [SerializeField] private string configExtension = "ini";
        
        [SerializeField] private ConfigDebugDataSourceSettings debug = new();

        public ConfigFolder Folder => folder;
        public string ConfigExtension => configExtension;
        public ConfigDebugDataSourceSettings Debug => debug;
    }
}