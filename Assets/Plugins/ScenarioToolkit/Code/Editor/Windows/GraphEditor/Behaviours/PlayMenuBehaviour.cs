using System;
using Scenario.Core.DataSource;
using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Core.Services;
using UnityEngine;
using UnityEngine.UIElements;
using ZLinq;

namespace ScenarioToolkit.Editor.Windows.GraphEditor.Behaviours
{
    public class PlayMenuBehaviour
    {
        private readonly GraphEditorWindow editorWindow;
        private readonly VisualElement root;
        private Toggle useNetwork, useLog;

        public PlayMenuBehaviour(GraphEditorWindow editorWindow, VisualElement root)
        {
            this.editorWindow = editorWindow;
            this.root = root;
        }
        public void UpdateUI() // TODO не совпадает с архитектурой, придумать как его сделать private
        {
            var playMenu = root?.Q<Foldout>("play-menu");
            if (playMenu == null) return;
            if (!Application.isPlaying)
            {
                playMenu.RemoveFromHierarchy();
                return;
            }

            var played = editorWindow.GraphHighlight.Player is { IsPlayed: true };
            var playName = played ? "Stop" : "Play";
            Action playAction = played ? Stop : Play;
            var playBtn = root.Q<Button>("play");
            
            //graphMenu.value = true;
            playBtn.text = playName;
            playBtn.clicked -= Play;
            playBtn.clicked -= Stop;
            playBtn.clicked += playAction;
            useNetwork = root.Q<Toggle>("use-network");
            useLog = root.Q<Toggle>("use-log");
        }
        
        public void Play()
        {
            if (!Application.isPlaying)
            {
                Debug.LogWarning("Can't play in edit mode");
                return;
            }
            
            var graphHighlightBehaviour = editorWindow.GraphHighlight;
            graphHighlightBehaviour.UpdatePlayer();
            
            if (graphHighlightBehaviour.Player == null)
            {
                if (EditorDiContainerService.Container == null)
                {
                    Debug.LogWarning($"Unresolved container in {nameof(EditorDiContainerService)}");
                    return;
                }
                
                editorWindow.ResolveGraph();
                
                if (graphHighlightBehaviour.Player == null)
                {
                    Debug.LogWarning($"Can't find ScenarioPlayer");
                    return;
                }
            }
            
            // Блок валидации (только для редактора)
            editorWindow.FunctionsMenu.DetectLoop(false);
            CheckStartEndNodes();
            
            var model = GetLaunchModel();
            SWEContext.Element.OverridesController.Save(false);
            
            graphHighlightBehaviour.Player.Play(editorWindow.ModelController.Model.Graph, editorWindow.ModelController.Model.Context, model);
            graphHighlightBehaviour.UpdatePlayer();
        }
        public void Stop()
        {
            editorWindow.GraphHighlight.Player?.Stop();
            editorWindow.GraphHighlight.UpdateNullPlayer();
        }
        
        private ScenarioLaunchModel GetLaunchModel()
        {
            var identityService = editorWindow.IdentityService;
            var identityHash = identityService != null && identityService.SelfIdentity
                ? identityService.SelfIdentity.AssetHashCode : 0;
            var model = new ScenarioLaunchModel
            {
                UseNetwork = useNetwork.value,
                UseLog = useLog.value,
                IdentityHash = identityHash,
            };
            if (!editorWindow.Modules) return model;
            
            var module = editorWindow.Modules.FirstOrDefault(editorWindow.Session.TextAsset);
            if (!module) return model;
            
            model.Scenario = module.ModuleIdentifier;
            return model;
        }
        private void CheckStartEndNodes()
        {
            var graph = editorWindow.ModelController.Model.Graph;
            if (!graph.NodesValuesAVE.OfType<IStartNode>().Any())
                Debug.LogWarning("Can't find any StartNode in scenario");
            if (!graph.NodesValuesAVE.OfType<IEndNode>().Any())
                Debug.LogWarning("Can't find any EndNode in scenario");
        }
    }
}