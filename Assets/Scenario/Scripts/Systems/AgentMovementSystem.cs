using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Scenario.Components.Actions.Unity;
using Scenario.Components.Conditions;
using Scenario.Core.Systems;
using UnityEngine;
using Zenject;

namespace Scenario.Systems
{
    public class AgentMovementSystem : BaseScenarioSystem
    {
        private CancellationTokenSource cancellationTokenSource;
        
        public AgentMovementSystem(SignalBus bus) : base(bus)
        {
            bus.Subscribe<SetAgentDestination>(SetAgentDestination);
        }

        private async void SetAgentDestination(SetAgentDestination component)
        {
            var agent = component.Agent;
            agent.enabled = true;
            cancellationTokenSource = new CancellationTokenSource();
            agent.speed = component.Speed;
            agent.SetDestination(component.Destination.position);

            try
            {
                await UniTask.WaitUntil(() =>
                    {
                        if (!agent.isOnNavMesh)
                        {
                            cancellationTokenSource.Cancel();
                            return true;
                        }

                        if (agent.pathPending)
                            return false;
                        return agent.remainingDistance <= 0.1f;
                    }, cancellationToken: cancellationTokenSource.Token)
                    .AttachExternalCancellation(cancellationTokenSource.Token);
            
                agent.ResetPath();
                Bus.Fire(new AgentReachedDestination() { Agent = agent });
                if (component.DisableAgentAfterDestinationReached)
                    agent.enabled = false;
            }
            catch (OperationCanceledException e)
            {
                Debug.LogWarning(e);
            }
        }
    }
}