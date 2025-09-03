using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Mirror;
using Scenario.Core.Serialization;
using VRF.Networking.Core;
using VRF.Players.Scriptables;
using Zenject;

namespace Scenario.Core.Systems.States
{
    public class ScenarioStateContainer : IInitializable, IDisposable
    {
        [CanBeNull] private readonly VRFNetworkManager networkManager;
        private readonly ScenarioSerializationService serializationService;
        private readonly Dictionary<Type, IScenarioStateProvider> providers = new();
        
        public ScenarioStateContainer([InjectOptional] VRFNetworkManager networkManager, 
            IEnumerable<IScenarioStateProvider> providersInstances, ScenarioSerializationService serializationService)
        {
            this.networkManager = networkManager;
            this.serializationService = serializationService;

            foreach (var scenarioStateProvider in providersInstances)
                providers.Add(scenarioStateProvider.GetState().GetType(), scenarioStateProvider);
        }
        public void Initialize()
        {
            if (!networkManager) return;
            networkManager.RegisterClientMessage<StatesMessage>(StatesMessageReceived);
            networkManager.OnServerClientAuthorized += NetworkManager_OnServerClientAuthorized;
        }
        public void Dispose()
        {
            if (!networkManager) return;
            networkManager.UnregisterClientMessage<StatesMessage>();
            networkManager.OnServerClientAuthorized -= NetworkManager_OnServerClientAuthorized;
        }

        private void StatesMessageReceived(StatesMessage message)
        {
            var states = serializationService.DeserializeBytes<IEnumerable<IState>>(message.StatesBytes);
            
            foreach (var state in states)
                providers[state.GetType()].SetState(state);
        }
        private void NetworkManager_OnServerClientAuthorized(NetworkConnectionToClient conn, PlayerIdentityConfig identity)
        {
            var states = providers.Values.Select(value => value.GetState());
            var bytes = serializationService.SerializeBytes(states);
            
            conn.Send(new StatesMessage { StatesBytes = bytes });
        }
        
        private void ClearStates()
        {
            foreach (var provider in providers)
                provider.Value.GetState().Clear();
        }
    }
}