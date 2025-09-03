using System;
using Mirror;
using UnityEngine;
using VRF.Networking.Messages;
using VRF.Scenes.Project;
using Zenject;

namespace VRF.Networking.Core.Client
{
    public class ClientNetScenesService : IInitializable, IDisposable
    {
        private readonly VRFAuthenticator authenticator;
        private readonly ScenesService service;

        public ClientNetScenesService(VRFAuthenticator authenticator, ScenesService service)
        {
            this.authenticator = authenticator;
            this.service = service;
        }
        
        public void Initialize()
        {
            authenticator.OnClientAuthenticated.AddListener(Send);
            service.OnSceneUpdated += Send;
        }
        public void Dispose()
        {
            authenticator.OnClientAuthenticated.RemoveListener(Send);
            service.OnSceneUpdated -= Send;
        }
        
        /// <summary>
        /// Отправка сообщения, что сцена изменена.
        /// Если загружена сцена, которая не зарегистрирована
        /// в сценах, то он пакет не отправит 
        /// (Bootstrap и Transition не входят в валидные сцены)
        /// </summary>
        private void Send()
        {
            if (!service.IsValidSceneIndex())
            {
                Debug.LogWarning($"Invalid scene index, index={service.SceneIndex}");
                return;
            }
            
            var message = new SceneUpdate_Message 
                { SceneIndex = service.SceneIndex };
            
            NetworkClient.Send(message);
        }
    }
}