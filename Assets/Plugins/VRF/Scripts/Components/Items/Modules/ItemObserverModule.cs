using System;
using System.IO;
using Mirror;
using NaughtyAttributes;
using Unity.Properties;
using UnityEngine;
using VRF.Components.Items.Views;
using VRF.Networking.Core;
using VRF.Networking.Messages;
using VRF.Players.Services.Views;
using VRF.Utilities;
using VRF.Utilities.Extensions;
using Zenject;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace VRF.Components.Items.Modules
{
    public class ItemObserverModule : BaseModule
    {
        [SerializeField] private bool activeNetwork = true;
        [SerializeField] private NetItemView netViewSource;
        public NetItemView NetViewSource => netViewSource;
        
        [InjectOptional] private ViewsSpawnedContainer spawnedContainer;
        [InjectOptional] private VRFNetworkManager networkManager;
        
        public override void Initialize()
        {
            if (spawnedContainer == null) return;
            if (networkManager == null) return;
            if (!activeNetwork) return;
            base.Initialize();
            
            spawnedContainer.OnAdd += OnAdd;
            
            if (networkManager.ClientConnected) SendSpawn();
            else networkManager.OnClientAuthorized += OnClientConnectedEvent;
            
        }
        public override void Dispose()
        {
            if (spawnedContainer == null) return;
            if (networkManager == null) return;
            if (!activeNetwork) return;
            base.Dispose();
            
            SendDestroy();
        }

        private void OnEnable()
        {
            if (!Initialized) return;
            netViewSource.SendEnable();
        }
        private void OnDisable()
        {
            if (!Initialized) return;
            netViewSource.SendDisable();
        }

        private void OnClientConnectedEvent()
        {
            networkManager.OnClientAuthorized -= OnClientConnectedEvent;
            SendSpawn();
        }
        private void OnAdd(BaseView view)
        {
            if (view is not NetItemView netView) return;
            if (View is not ItemView selfView) return;
            spawnedContainer.OnAdd -= OnAdd;
            
            netView.NetBehaviour.OnStartClientCallOrSubscribe(OnInitialize);
            return;

            void OnInitialize()
            {
                if (netView.NetBehaviour.isOwned)
                    netView.LocalInitialize(selfView);
            }
        }

        private void SendSpawn()
        {
            var msg = new NetItemSpawn_RequestMessage
            {
                AssetHashCode = netViewSource.AssetHashCode,
                RuntimeHashCode = netViewSource.RuntimeHashCode,
            };
            if (NetworkClient.active) NetworkClient.Send(msg);
        }
        private void SendDestroy()
        {
            var msg = new NetItemDestroy_RequestMessage
            {
                AssetHashCode = netViewSource.AssetHashCode,
                RuntimeHashCode = netViewSource.RuntimeHashCode,
            };
            if (NetworkClient.active) NetworkClient.Send(msg);
        }

        #region Editor
        private bool ShowInstantiateButton => !netViewSource;

        [ShowIf(nameof(ShowInstantiateButton))]
        [Button]
        private void FindOrCreateNewNetCopy()
        {
#if UNITY_EDITOR
            var path = gameObject.GetPath();
            var newPath = GetNewNetPath(path);
            var newObj = AssetDatabase.LoadAssetAtPath<GameObject>(newPath);
            
            if (!newObj)
            {
                if (!AssetDatabase.CopyAsset(path, newPath))
                    throw new InvalidPathException($"Can't copy from {path} to {newPath}");
                newObj = AssetDatabase.LoadAssetAtPath<GameObject>(newPath);
            }

            newObj.EnsureComponent<NetItemReceiverModule>(out var module);
            netViewSource = module.ValidateReceiverComponents();
            VrfRuntimeEditor.SetDirty(this);
#endif
        }

        private string GetNewNetPath(string path)
        {
            var info = new DirectoryInfo(path);
            if (info.Parent == null) throw new DirectoryNotFoundException();

            var newName = $"Net {gameObject.name}";
            var newFileName = info.Name.Replace(gameObject.name, newName);
            var newDirectory = Path.Combine(info.Parent.ToString(), "Network");
            if (!Directory.Exists(newDirectory)) Directory.CreateDirectory(newDirectory);

            var newPath = Path.Combine(newDirectory, newFileName);
            newPath = newPath.Replace('\\', '/');
            if (newPath.StartsWith(Application.dataPath))
                newPath = "Assets" + newPath.Substring(Application.dataPath.Length);
            return newPath;
        }
        #endregion
    }
}