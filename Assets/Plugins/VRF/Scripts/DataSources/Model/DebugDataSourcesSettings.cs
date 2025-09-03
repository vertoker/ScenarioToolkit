using System;
using VRF.DataSources.CommandLine;
using VRF.DataSources.Config;
using VRF.DataSources.LocalCache;
using VRF.DataSources.Scriptables;

namespace VRF.DataSources.Model
{
    [Serializable]
    public class ContainerDebugDataSourceSettings : BaseDebugDataSourceSettings<ContainerDataSource>
    {
    }

    [Serializable]
    public class ConfigDebugDataSourceSettings : BaseDebugDataSourceSettings<ConfigDataSource>
    {
    }

    [Serializable]
    public class CommandLineDebugDataSourceSettings : BaseDebugDataSourceSettings<CommandLineDataSource>
    {
    }

    [Serializable]
    public class ScriptableDebugDataSourceSettings : BaseDebugDataSourceSettings<ScriptableDataSource>
    {
    }

    [Serializable]
    public class LocalCacheDebugDataSourceSettings : BaseDebugDataSourceSettings<LocalCacheDataSource>
    {
    }
    //public class CustomDebugDataSourceSettings : BaseDebugDataSourceSettings<CustomDataSource> { }
}