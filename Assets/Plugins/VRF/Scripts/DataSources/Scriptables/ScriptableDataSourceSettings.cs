using UnityEngine;
using VRF.DataSources.Model;

namespace VRF.DataSources.Scriptables
{
    [System.Serializable]
    public class ScriptableDataSourceSettings
    {
        [SerializeField] private ScriptableDebugDataSourceSettings debug = new();

        public ScriptableDebugDataSourceSettings Debug => debug;
    }
}