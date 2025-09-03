using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using VRF.Scenes.Project;
using VRF.Scenes.Scriptables;

namespace VRF.Editor.TestBase
{
    public static class ScenesExtensions
    {
        public static ClientScenesConfig GetScenesConfigs(this BaseTests tests)
        {
            var installer = BaseZenjectContextTests.GetInstaller<ScenesServiceInstaller>(BaseZenjectContextTests.GetProjectContext());
            if (installer == null)
                tests.LogError($"Can't find installer, search or add {nameof(ScenesServiceInstaller)}", installer);
            
            if (!installer.ScenesConfig)
                tests.LogError(BaseZenjectContextTests.NullInstallerConfigMessage<ScenesServiceInstaller, ClientScenesConfig>(),
                    installer);
            var scenesConfigs = installer.ScenesConfig;
            return scenesConfigs;
        }
        public static IEnumerable<Scene> ClientScenesIterator(this ClientScenesConfig scenesConfig)
        {
            foreach (var scenePath in scenesConfig.Scenes)
            {
                var scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Additive);
                yield return scene;
                EditorSceneManager.CloseScene(scene, true);
            }
        }
    }
}