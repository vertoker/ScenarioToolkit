using System;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using Newtonsoft.Json;
using ScenarioToolkit.Core.Serialization;
using ScenarioToolkit.Core.World;
using ScenarioToolkit.Editor.Utilities;
using ScenarioToolkit.Shared.VRF.Utilities;
using UnityEditor;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace Scenario.Editor.Model
{
    /// <summary>
    /// Дополнительная модель, которая хранит дополнительные данные для редактора,
    /// сохраняя текущую сессию с загруженным сценарием
    /// </summary>
    public class ScenarioGraphEditorSession
    {
        [JsonProperty("LastOpenedPath")]
        public string LastOpenedPath { get; private set; } = string.Empty;
        public List<string> PathStack = new();
        public List<int> HashStack = new();
        public Dictionary<string, GraphCameraPose> LastCameraPoses = new();
        public string SceneContextID = string.Empty;
        
        //[JsonIgnore, CanBeNull] public ScenarioPlayer SessionPlayer { get; private set; }
        [JsonIgnore] public TextAsset TextAsset { get; private set; }
        [JsonIgnore] private ScenarioSerializationService serializationService;
        
        public event Action<string> FileNameChanged;
        
        
        // SceneContext Zone

        public void SetSceneContext([CanBeNull] IScenarioWorldID sceneContext)
        {
            SceneContextID = sceneContext == null ? string.Empty : sceneContext.GetID();
        }
        public void ResetSceneContext()
        {
            SceneContextID = string.Empty;
        }
        
        
        // Subgraph Stack Zone
        
        public void AddCurrent(int nodeHash) => AddToStack(LastOpenedPath, nodeHash);
        private void AddToStack(string path, int nodeHash)
        {
            PathStack.Add(path);
            HashStack.Add(nodeHash);
        }
        public void UpdateStack(string lastOpenedPath)
        {
            var index = PathStack.IndexOf(lastOpenedPath);
            if (index == -1) return;
            PathStack.RemoveRange(index, PathStack.Count - index);
            HashStack.RemoveRange(index, HashStack.Count - index);
        }
        public void ClearStack()
        {
            PathStack.Clear();
            HashStack.Clear();
        }
        
        
        // Last Camera Poses Zone

        public void UpdatePose(string path, GraphCameraPose newPose)
        {
            if (string.IsNullOrWhiteSpace(path)) return;
            LastCameraPoses[path] = newPose;
        }
        public GraphCameraPose GetPose(string path) => LastCameraPoses.GetValueOrDefault(path);
        public void ClearLastPoses()
        {
            LastCameraPoses.Clear();
        }
        
        
        // Source File Zone

        public void RefreshSession(string filePath)
        {
            FileNameChanged?.Invoke(filePath);
            LastOpenedPath = filePath;
            
            LoadAsset();
            Save();
        }
        
        
        // Save Zone
        
        private static readonly string SessionFilePath = Path.Join(DirectoryFileHelper.EditorDirectory, "session.json");
        public void Save()
        {
            DirectoryFileHelper.ValidateDirectory(DirectoryFileHelper.EditorDirectory);
            DirectoryFileHelper.InitGitignore();
            var json = serializationService.Serialize(this);
            //Debug.Log($"Save session");
            File.WriteAllText(SessionFilePath, json);
        }
        public static ScenarioGraphEditorSession Load(ScenarioSerializationService serializationService = null)
        {
            serializationService ??= new ScenarioSerializationService();
            ScenarioGraphEditorSession session;
            
            try
            {
                var data = File.ReadAllText(SessionFilePath);
                session = serializationService.Deserialize<ScenarioGraphEditorSession>(data);
                
                // Compatibility
                session.PathStack ??= new List<string>();
                session.HashStack ??= new List<int>();
                session.SceneContextID ??= string.Empty;
                session.LastCameraPoses ??= new Dictionary<string, GraphCameraPose>();
            }
            catch (Exception)
            {
                Debug.LogWarning("Can't load session, fallback to default");
                session = new ScenarioGraphEditorSession();
            }

            session.serializationService = serializationService;
            session.LoadAsset();
            return session;
        }
        private void LoadAsset()
        {
            if (string.IsNullOrEmpty(LastOpenedPath))
            {
                TextAsset = null;
                return;
            }
            
            var projectPath = VrfPath.FromAbsoluteToProject(LastOpenedPath);
            TextAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(projectPath);
            //Debug.Log(TextAsset.name);
        }
    }
}