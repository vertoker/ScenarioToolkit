using System;
using UnityEngine;
using VRF.DataSources.CommandLine;

namespace VRF.Identities.Models
{
    /// <summary>
    /// Реализация AuthIdentityModel для CommandLine
    /// </summary>
    public class AuthIdentityModelCmdParser : IModelCmdParser
    {
        public bool CanParseModel(CommandLineParser parser)
        {
            return parser.ContainsAny("--authmode", "-am")
                   && parser.ContainsAny("--nickname", "-nn")
                   && parser.ContainsAny("--password", "-pw")
                   && parser.ContainsAny("--id", "-id");
        }

        public object ParseModel(CommandLineParser parser)
        {
            var model = new AuthIdentityModel();
            
            if (parser.GetDataByAnyTag(out var authModeStr, "--authmode", "-am"))
            {
                if (Enum.TryParse<IdentityAuthMode>(authModeStr, out var mode))
                    model.AuthMode = mode;
                else
                    Debug.LogWarning($"<b>{nameof(AuthIdentityModelCmdParser)}</b> can't parse " +
                                     $"<b>{nameof(IdentityAuthMode)}</b>. Parsed string is <b>{authModeStr}</b>, " +
                                     $"you maybe mistake in command line name parameter. " +
                                     $"Model set <b>{nameof(IdentityAuthMode.Nickname)}</b> as default");
            }
            
            if (parser.GetDataByAnyTag(out var nickname, "--nickname", "-nn"))
                model.IdentityName = nickname;
            if (parser.GetDataByAnyTag(out var password, "--password", "pw"))
                model.Password = password;
            if (parser.GetDataByAnyTag(out var idStr, "--id", "-id"))
                model.IdentityID = int.Parse(idStr);
            
            return !string.IsNullOrEmpty(authModeStr) 
                   && (!string.IsNullOrEmpty(nickname) || !string.IsNullOrEmpty(password) || !string.IsNullOrEmpty(idStr))
                ? model : null;
        }

        public Type GetModelType() => typeof(AuthIdentityModel);
    }
}