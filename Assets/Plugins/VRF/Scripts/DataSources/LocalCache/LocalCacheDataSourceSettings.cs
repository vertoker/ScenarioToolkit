using System;
using UnityEngine;
using VRF.DataSources.Model;

namespace VRF.DataSources.LocalCache
{
    [Serializable]
    public class LocalCacheDataSourceSettings
    {
        [SerializeField] private LocalCacheDebugDataSourceSettings debug = new();

        public LocalCacheDebugDataSourceSettings Debug => debug;
    }
}