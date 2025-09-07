using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace ScenarioToolkit.Editor.Windows
{
    /// <summary>
    /// Базовое сценарное окно, от него обязаны наследуются все сценарно-ориентированные окна
    /// (это те, что участвуют в прямом редактировании сценариев)
    /// </summary>
    public class BaseScenarioWindow : EditorWindow
    {
        // Не сказал бы что это решение оптимально, но всё же свою задачу оно решает
        private static readonly List<BaseScenarioWindow> ScenarioWindows = new();
        
        public void SetDirtyScenario()
        {
            hasUnsavedChanges = true;
            //Debug.Log("Dirty");
            //foreach (var scenarioWindow in scenarioWindows)
            //    scenarioWindow.SetDirtyScenario();
        }

        public bool IsHasAnyUnsavedChanges() 
            => hasUnsavedChanges || ScenarioWindows.Any(w => w.hasUnsavedChanges);

        public virtual void Save()
        {
            SaveWindow();
            foreach (var scenarioWindow in ScenarioWindows)
                scenarioWindow.SaveWindow();
        }
        protected virtual void SaveWindow()
        {
            hasUnsavedChanges = false;
        }

        protected void BindWindow(BaseScenarioWindow other)
        {
            ScenarioWindows.Add(other);
            //other.ScenarioWindows.Add(this);
        }
        protected void UnbindWindow(BaseScenarioWindow other)
        {
            ScenarioWindows.Remove(other);
            //other.ScenarioWindows.Remove(this);
        }

        protected virtual void OnDestroy()
        {
            for (var i = ScenarioWindows.Count - 1; i >= 0; i--)
                UnbindWindow(ScenarioWindows[i]);
        }
        protected virtual void OnEnable()
        {
            EditorSceneManager.sceneOpened += OnSceneOpened;
            EditorSceneManager.sceneClosing += OnSceneClosed;
            EditorSceneManager.sceneSaving += OnSceneSaved;
        }
        protected virtual void OnDisable()
        {
            EditorSceneManager.sceneOpened -= OnSceneOpened;
            EditorSceneManager.sceneClosing -= OnSceneClosed;
            EditorSceneManager.sceneSaving -= OnSceneSaved;
        }
        
        protected virtual void OnSceneOpened(Scene scene, OpenSceneMode mode)
        {
            //Debug.Log(nameof(OnSceneOpened));
        }
        protected virtual void OnSceneClosed(Scene scene, bool removingScene)
        {
            //Debug.Log(nameof(OnSceneClosed));
        }
        protected virtual void OnSceneSaved(Scene scene, string path)
        {
            //Debug.Log(nameof(OnSceneSaved));
            Save();
        }
    }
}