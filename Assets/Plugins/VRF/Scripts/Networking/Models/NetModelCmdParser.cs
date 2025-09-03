using System;
using Mirror;
using UnityEngine;
using VRF.DataSources.CommandLine;

namespace VRF.Networking.Models
{
    /// <summary>
    /// Реализация NetModel для CommandLine
    /// </summary>
    public class NetModelCmdParser : IModelCmdParser
    {
        public bool CanParseModel(CommandLineParser parser)
        {
            return parser.ContainsAny("--netmode", "-nm")
                   && parser.ContainsAny("--address", "-ip")
                   && parser.ContainsAny("--password", "-pw")
                   && parser.ContainsAny("--port", "-p");
        }
        
        public object ParseModel(CommandLineParser parser)
        {
            var model = new NetModel();

            if (parser.GetDataByAnyTag(out var modeStr, "--netmode", "-nm"))
            {
                if (Enum.TryParse<NetworkManagerMode>(modeStr, out var mode))
                    model.NetMode = mode;
                else
                    Debug.LogWarning($"<b>{nameof(NetModelCmdParser)}</b> can't parse " +
                                     $"<b>{nameof(NetworkManagerMode)}</b>. Parsed string is <b>{modeStr}</b>, " +
                                     $"you maybe mistake in command line name parameter. " +
                                     $"Model set <b>{nameof(NetworkManagerMode.Host)}</b> as default");
            }

            if (parser.GetDataByAnyTag(out var address, "--address", "-ip"))
                model.Address = address;
            if (parser.GetDataByAnyTag(out var portStr, "--port", "-p"))
                model.Port = ushort.Parse(portStr);
            
            return string.IsNullOrEmpty(modeStr) 
                   || string.IsNullOrEmpty(address) 
                   || string.IsNullOrEmpty(portStr)
                ? null : model;
        }

        public Type GetModelType() => typeof(NetModel);
    }
}