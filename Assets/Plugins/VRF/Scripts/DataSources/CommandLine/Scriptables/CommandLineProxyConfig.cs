using NaughtyAttributes;
using UnityEngine;

namespace VRF.DataSources.CommandLine.Scriptables
{
    /// <summary>
    /// Editor имплементация командной строки через другую имплементацию командной строки.
    /// Единственная причина для существования данного конфига это попытка не трогать ProjectContext
    /// </summary>
    [CreateAssetMenu(fileName = nameof(CommandLineProxyConfig), menuName = "VRF/DataSources/" + nameof(CommandLineProxyConfig))]
    public class CommandLineProxyConfig : BaseCommandLineConfig
    {
        [Expandable]
        [SerializeField] private BaseCommandLineConfig source;
        
        public override string[] GetArgs() => source.GetArgs();
    }
}