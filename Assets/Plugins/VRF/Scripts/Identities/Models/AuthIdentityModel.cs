using System;
using Mirror;
using NaughtyAttributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;
using VRF.Networking.Messages;

namespace VRF.Identities.Models
{
    /// <summary>
    /// Режим идентификации игрока при авторизации
    /// </summary>
    public enum IdentityAuthMode
    {
        Nickname = 0,
        NicknamePassword = 1,
        ID = 2
    }
    
    /// <summary>
    /// Модель для идентификации игрока. Встроен в систему DataSources.
    /// Настроен на правильную сериализацию в JSON
    /// </summary>
    [Serializable]
    public class AuthIdentityModel
    {
        #region Data
        [JsonConverter(typeof(StringEnumConverter))]
        [field:SerializeField] public IdentityAuthMode AuthMode { get; set; }
        
        
        [field:AllowNesting, HideIf(nameof(ByID))]
        [field:SerializeField] public string IdentityName { get; set; }
        
        
        [field:AllowNesting, ShowIf(nameof(ByNicknameAndPassword))]
        [field:SerializeField] public string Password { get; set; }
        
        
        [field:AllowNesting, ShowIf(nameof(ByID))]
        [field:SerializeField] public int IdentityID { get; set; }
        
        [JsonIgnore] public bool ByNickname => AuthMode == IdentityAuthMode.Nickname;
        [JsonIgnore] public bool ByNicknameAndPassword => AuthMode == IdentityAuthMode.NicknamePassword;
        [JsonIgnore] public bool ByID => AuthMode == IdentityAuthMode.ID;
        #endregion
        
        #region Constructors
        public AuthIdentityModel()
        {
            AuthMode = IdentityAuthMode.Nickname;
        }
        public AuthIdentityModel(string identityName)
        {
            AuthMode = IdentityAuthMode.Nickname;
            IdentityName = identityName;
        }
        public AuthIdentityModel(string identityName, string password)
        {
            AuthMode = IdentityAuthMode.NicknamePassword;
            IdentityName = identityName;
            Password = password;
        }
        public AuthIdentityModel(int identityID)
        {
            AuthMode = IdentityAuthMode.ID;
            IdentityID = identityID;
        }
        #endregion

        #region Messages
        public AuthNickname_RequestMessage GetNickname() => new() { Nickname = IdentityName };
        public AuthNicknamePassword_RequestMessage GetNicknamePassword() => new() { Nickname = IdentityName, Password = Password };
        public AuthIDRequest_Message GetID() => new() { ID = IdentityID };

        public bool Compare<TAuthMessage>(TAuthMessage msg) where TAuthMessage : struct, NetworkMessage
        {
            switch (AuthMode)
            {
                case IdentityAuthMode.Nickname:
                    if (msg is AuthNickname_RequestMessage nickname)
                        return nickname.Nickname == IdentityName;
                    return false;
                case IdentityAuthMode.NicknamePassword:
                    if (msg is AuthNicknamePassword_RequestMessage nicknamePassword)
                        return nicknamePassword.Nickname == IdentityName 
                               && nicknamePassword.Password == Password;
                    return false;
                case IdentityAuthMode.ID:
                    if (msg is AuthIDRequest_Message id)
                        return id.ID == IdentityID;
                    return false;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        public bool Compare(AuthIdentityModel model)
        {
            if (AuthMode != model.AuthMode) return false;

            return AuthMode switch
            {
                IdentityAuthMode.Nickname => IdentityName == model.IdentityName,
                IdentityAuthMode.NicknamePassword 
                    => IdentityName == model.IdentityName && Password == model.Password,
                IdentityAuthMode.ID => IdentityID == model.IdentityID,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        #endregion
    }
}