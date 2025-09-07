using JetBrains.Annotations;
using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Core.Player;
using UnityEngine;

namespace ScenarioToolkit.Editor.Windows.GraphEditor.Behaviours
{
    public class GraphHighlightBehaviour
    {
        private readonly GraphEditorWindow graphEditor;
        
        [CanBeNull] public ScenarioPlayer Player { get; private set; }
        
        private bool bind;

        public GraphHighlightBehaviour(GraphEditorWindow graphEditor)
        {
            this.graphEditor = graphEditor;
        }
        
        public void UpdatePlayer()
        {
            UnbindPlayer();
            UpdatePlayerImpl();
            BindPlayer();
            graphEditor.PlayMenu.UpdateUI();
        }
        public void UpdateNullPlayer()
        {
            UnbindPlayer();
            Player = null;
            BindPlayer();
            graphEditor.PlayMenu.UpdateUI();
        }

        private void UpdatePlayerImpl()
        {
            if (graphEditor.Session == null)
                Player = null;
            else if (string.IsNullOrEmpty(graphEditor.Session.SceneContextID)) // Container
                Player = GetOpenedPlayer(graphEditor.ContainerPlayer);
            else // Instance
            {
                var instance = graphEditor.SceneProvider?.GetScenarioInstance(graphEditor.Session.SceneContextID);
                Player = instance ? GetOpenedPlayer(instance.Player) : null;
            }

            //Debug.Log("VAR");
        }
        private ScenarioPlayer GetOpenedPlayer(ScenarioPlayer parent)
        {
            if (parent is not { IsPlayed: true }) return parent;
            
            foreach (var hash in graphEditor.Session.HashStack)
            {
                var node = parent.Graph.GetNode(hash);
                if (node is not ISubgraphNode subgraphNode)
                {
                    Debug.LogWarning($"Can't find node {hash} in player {parent.Hash}");
                    return parent;
                }
                
                if (subgraphNode.SubPlayer == null)
                    return parent;
                parent = subgraphNode.SubPlayer;
            }
            return parent;
        }
        
        private void UnbindPlayer()
        {
            if (Player == null) return;
            if (!bind) return;
            bind = false;
            
            //Debug.Log($"UnbindPlayer {sessionPlayer != null} {containerPlayer != null}");
            
            graphEditor.GraphController.ConditionNodeSkipped -= ConditionNodeSkipped;
            graphEditor.GraphController.SubgraphNodeSkipped -= SubgraphNodeSkipped;
            graphEditor.GraphController.NodeActivated -= Player.Activate;
            //player.ScenarioStarted -= graphEditor.PlayerStarted;
            //player.ScenarioStopped -= graphEditor.PlayerStopped;
            
            graphEditor.GraphView.ResetPlayer();
        }
        private void BindPlayer()
        {
            if (Player == null) return;
            if (bind) UnbindPlayer();
            bind = true;
            
            //Debug.Log($"BindPlayer {sessionPlayer != null} {containerPlayer != null}");
            
            graphEditor.GraphController.ConditionNodeSkipped += ConditionNodeSkipped;
            graphEditor.GraphController.SubgraphNodeSkipped += SubgraphNodeSkipped;
            graphEditor.GraphController.NodeActivated += Player.Activate;
            //player.ScenarioStarted += PlayerStarted;
            //player.ScenarioStopped += PlayerStopped;
            
            graphEditor.GraphView.SetPlayer(Player);
        }
        
        private void ConditionNodeSkipped(IConditionNode conditionNode) => conditionNode.ForceEnd();
        private void SubgraphNodeSkipped(ISubgraphNode subgraphNode) => subgraphNode.ForceEnd();
    }
}