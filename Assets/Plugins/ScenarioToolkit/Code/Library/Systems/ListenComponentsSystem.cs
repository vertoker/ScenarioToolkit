using System;
using System.Collections.Generic;
using Scenario.Core.Model;
using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Core.Player;
using ScenarioToolkit.Core.Systems;
using ScenarioToolkit.Shared;
using UnityEngine;
using Zenject;

namespace ScenarioToolkit.Library.Systems
{
    public class ListenComponentsSystem : BaseScenarioSystem
    {
        public Action<ComponentCallContext> ActionSended;
        public Action<ComponentCallContext> ConditionReceived;

        private readonly Dictionary<ScenarioPlayer, Entry> entries = new();

        public ListenComponentsSystem(SignalBus bus) : base(bus)
        {
            bus.Subscribe<StartListenComponentsContext>(StartListenComponentsContext);
            bus.Subscribe<StopListenComponentsContext>(StopListenComponentsContext);
        }

        private void StartListenComponentsContext(StartListenComponentsContext component)
        {
            if (AssertLog.NotNull<ListenComponentsSystem>(component.Player, nameof(component.Player))) return;
            if (AssertLog.IsFalse(entries.ContainsKey(component.Player), "Root Player is already listened")) return;

            var entryPlayer = new EntryPlayer(component.Player);
            var entry = new Entry(entryPlayer, OnScenarioStopped);
            entries.Add(component.Player, entry);
            
            entryPlayer.ActionSended += ActionSendedImpl;
            entryPlayer.ConditionReceived += ConditionReceivedImpl;
            return;

            void OnScenarioStopped()
            {
                Bus.Fire(component.GetStopContext());
            }
        }
        private void StopListenComponentsContext(StopListenComponentsContext component)
        {
            if (AssertLog.NotNull<ListenComponentsSystem>(component.Player, nameof(component.Player))) return;
            if (AssertLog.IsTrue(entries.TryGetValue(component.Player, out var entry),
                    "Root Player is already not listened")) return;

            // ReSharper disable once PossibleNullReferenceException
            entry.Dispose();
            entries.Remove(component.Player);
        }

        private void ActionSendedImpl(ScenarioPlayer player, IScenarioNodeFlow flowNode, int index, IScenarioAction component)
        {
            Debug.Log($"<b>ActionSended</b>: player:<b>{player.Hash}</b>, nodeHash:<b>{flowNode.Hash}</b>, " +
                      $"index:<b>{index}</b>, component:<b>{component.GetType().Name}</b>");
        }
        private void ConditionReceivedImpl(ScenarioPlayer player, IScenarioNodeFlow flowNode, int index, IScenarioCondition component)
        {
            Debug.Log($"<b>ConditionReceived</b>: player:<b>{player.Hash}</b>, nodeHash:<b>{flowNode.Hash}</b>, " +
                      $"index:<b>{index}</b>, component:<b>{component.GetType().Name}</b>");
        }
        
        private class Entry : IDisposable
        {
            public readonly EntryPlayer EntryPlayer;
            public readonly Action OnStopped;

            public Entry(EntryPlayer newEntryPlayer, Action newOnStopped)
            {
                EntryPlayer = newEntryPlayer;
                OnStopped = newOnStopped;
                EntryPlayer.Player.ScenarioStopped += OnStopped;
            }
            public void Dispose()
            {
                EntryPlayer.Player.ScenarioStopped -= OnStopped;
                EntryPlayer.Dispose();
            }
        }
        private class EntryPlayer : IDisposable
        {
            public Action<ScenarioPlayer, IScenarioNodeFlow, int, IScenarioAction> ActionSended;
            public Action<ScenarioPlayer, IScenarioNodeFlow, int, IScenarioCondition> ConditionReceived;

            public readonly ScenarioPlayer Player;
            public readonly Dictionary<int, EntryPlayer> SubEntries;
            public readonly Dictionary<int, EntryNode> EntryNodes;

            public EntryPlayer(ScenarioPlayer newPlayer)
            {
                Player = newPlayer;
                SubEntries = new Dictionary<int, EntryPlayer>();
                EntryNodes = new Dictionary<int, EntryNode>();

                Player.NodeBeforeActivated += NodeBeforeActivated;
                Player.NodeAfterCompleted += NodeAfterCompleted;
            }
            public void Dispose()
            {
                Player.NodeBeforeActivated -= NodeBeforeActivated;
                Player.NodeAfterCompleted -= NodeAfterCompleted;

                foreach (var entryNode in EntryNodes)
                    entryNode.Value.Dispose();
                foreach (var subEntry in SubEntries)
                    subEntry.Value.Dispose();
            }

            private void NodeBeforeActivated(IScenarioNodeFlow flowNode)
            {
                var entryNode = new EntryNode(this, flowNode);
                EntryNodes.Add(flowNode.Hash, entryNode);
            }
            private void NodeAfterCompleted(IScenarioNodeFlow flowNode)
            {
                if (EntryNodes.Remove(flowNode.Hash, out var entryNode))
                    entryNode.Dispose();
            }
        }
        private class EntryNode : IDisposable
        {
            private readonly EntryPlayer entry;
            private readonly IScenarioNodeFlow node;

            public EntryNode(EntryPlayer newEntry, IScenarioNodeFlow newNode)
            {
                entry = newEntry;
                node = newNode;

                switch (node)
                {
                    case IActionNode actionNode:
                        actionNode.ActionAfterFire += ActionAfterFire;
                        break;
                    case IConditionNode conditionNode:
                        conditionNode.ConditionCompleted += ConditionCompleted;
                        break;
                    case ISubgraphNode subgraphNode:
                        subgraphNode.OnSubgraphIsReady += SubgraphReady;
                        break;
                }
            }
            public void Dispose()
            {
                switch (node)
                {
                    case IActionNode actionNode:
                        actionNode.ActionAfterFire -= ActionAfterFire;
                        break;
                    case IConditionNode conditionNode:
                        conditionNode.ConditionCompleted -= ConditionCompleted;
                        break;
                    case ISubgraphNode subgraphNode:
                        subgraphNode.OnSubgraphIsReady -= SubgraphReady;
                        break;
                }
            }

            private void ActionAfterFire(IScenarioAction component, int index)
            {
                entry.ActionSended?.Invoke(entry.Player, node, index, component);
            }
            private void ConditionCompleted(IScenarioCondition component, int index)
            {
                entry.ConditionReceived?.Invoke(entry.Player, node, index, component);
            }
            private void SubgraphReady(ISubgraphNode subgraphNode)
            {
                var subEntry = new EntryPlayer(subgraphNode.SubPlayer);
                entry.SubEntries.Add(subgraphNode.Hash, subEntry);
                subEntry.ActionSended += entry.ActionSended;
                subEntry.ConditionReceived += entry.ConditionReceived;
            }
        }
    }

    public struct ComponentCallContext
    {
        public SendDirection Direction;
        public ScenarioPlayer Player;
        public bool IsSubPlayer;

        public int NodeHash;
        public int ComponentIndex;

        public enum SendDirection
        {
            None = 0,
            FromScenario = 1,
            ToScenario = 2,
        }
    }
}