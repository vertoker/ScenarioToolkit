using UnityEngine;

namespace VRF.DataSources.CommandLine.Scriptables
{
    /// <summary>
    /// Editor имитация командной строки, имеет свои реализации
    /// </summary>
    public abstract class BaseCommandLineConfig : ScriptableObject
    {
        public abstract string[] GetArgs();
    }
}