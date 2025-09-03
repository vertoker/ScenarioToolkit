using UnityEngine;

namespace VRF.DataSources.CommandLine.Scriptables
{
    /// <summary>
    /// Стандартная Editor имплементация командной строки через string массив
    /// </summary>
    [CreateAssetMenu(fileName = nameof(CommandLineArgsConfig), menuName = "VRF/DataSources/" + nameof(CommandLineArgsConfig))]
    public class CommandLineArgsConfig : BaseCommandLineConfig
    {
        [SerializeField] private string[] args;
        
        public override string[] GetArgs() => args;
    }
}