using System;
using UnityEngine;
using VRF.DataSources.CommandLine;

namespace VRF.Players.Models.Player
{
    public class PlayerModelCmdParser : IModelCmdParser
    {
        public bool CanParseModel(CommandLineParser parser)
        {
            return parser.ContainsAny("--controlmode", "-cm");
        }
        
        public object ParseModel(CommandLineParser parser)
        {
            var model = new PlayerModel();
            
            if (parser.GetDataByAnyTag(out var modeStr, "--controlmode", "-cm"))
            {
                if (Enum.TryParse<PlayerControlModes>(modeStr, out var mode))
                    model.PriorityControlMode = mode;
                else
                    Debug.LogWarning($"{nameof(PlayerModelCmdParser)} can't parse " +
                                     $"{nameof(PlayerControlModes)}. Parsed string is {modeStr}, " +
                                     $"you maybe mistake in command line name parameter. " +
                                     $"Model set {nameof(PlayerControlModes.VR)} as default");
            }
            
            return model;
        }

        public Type GetModelType() => typeof(PlayerModel);
    }
}