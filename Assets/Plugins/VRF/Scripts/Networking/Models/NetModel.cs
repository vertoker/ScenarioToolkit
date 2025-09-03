using System;
using Mirror;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;

namespace VRF.Networking.Models
{
    /// <summary>
    /// Модель со всеми данными, чтобы запустить сервер в определённом режиме.
    /// Используется в DataSources. Оптимизирован для сериализации в JSON
    /// </summary>
    [Serializable]
    public class NetModel
    {
        /// <summary> Режим, в котором запуститься сетевой клиент.
        /// Может быть Offline, ServerOnly, ClientOnly, Host </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        [field:SerializeField] public NetworkManagerMode NetMode { get; set; } = NetworkManagerMode.Host;
        
        /// <summary> Адрес, к которому подключается клиент/с которого запускается сервер
        /// (хост = сервер + клиент, подключение идёт к самому себе) </summary>
        [field:SerializeField] public string Address { get; set; } = "localhost";
        
        /// <summary> Порт, по которому подключается клиент/с которого запускается сервер
        /// (хост = сервер + клиент, подключение идёт к самому себе) </summary>
        [field:SerializeField] public ushort Port { get; set; } = 7777;
    }
}