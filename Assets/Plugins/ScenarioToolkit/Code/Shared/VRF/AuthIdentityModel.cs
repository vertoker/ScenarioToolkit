using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;

namespace ScenarioToolkit.Shared.VRF
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
        
        [field:SerializeField] public string IdentityName { get; set; }
        [field:SerializeField] public string Password { get; set; }
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