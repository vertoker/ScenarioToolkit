using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRF.Utilities.Extensions;
using Zenject;

namespace VRF.Editor.TestBase
{
    public abstract class BaseZenjectContextTests : BaseTests
    {
        public static ProjectContext GetProjectContext() => FindComponents<ProjectContext>().First();

        public static SceneContext GetSceneContext(Scene scene) => scene.FindComponent<SceneContext>();
        public static TInstaller GetInstaller<TInstaller>(Context context) where TInstaller : MonoInstaller 
            => (TInstaller)context.Installers.FirstOrDefault(i => i.GetType() == typeof(TInstaller));
        
        public static string NullInstallerConfigMessage<TInstaller, TConfig>()
            => $"Null reference of {typeof(TConfig).Name} in {typeof(TInstaller).Name}, create {typeof(TConfig).Name}";
        
        /// <summary>
        /// Пишет обычный LogError, но также запоминает, что есть ошибка
        /// </summary>
        public void PrintError(string message, Scene scene, Object context = null)
        {
            if (!ThrowError)
                LogError($"<Errors on {scene.name} scene> you can find it in {scene.path} path", context);
            LogError(message, context);
        }
    }
}