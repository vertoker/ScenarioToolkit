using UnityEngine;

namespace VRF.DataSources
{
    public static class DataSourceStatic
    {
        public static readonly DataSourceType[] DefaultEditor = {
            DataSourceType.LocalCache,
            DataSourceType.Scriptable,
            DataSourceType.Custom,
        };
        public static readonly DataSourceType[] DefaultRuntime = {
            DataSourceType.LocalCache,
            DataSourceType.CommandLine,
            DataSourceType.ConfigParser,
            DataSourceType.Custom,
        };
        public static readonly DataSourceType[] DefaultAll = {
            DataSourceType.LocalCache,
            DataSourceType.Scriptable,
            DataSourceType.CommandLine,
            DataSourceType.ConfigParser,
            DataSourceType.Custom,
        };
        
        public static DataSourceType[] GetSources(DataSourceType[] sourcesEditor, DataSourceType[] sourcesRuntime) =>
            Application.isEditor ? sourcesEditor : sourcesRuntime;
    }
}