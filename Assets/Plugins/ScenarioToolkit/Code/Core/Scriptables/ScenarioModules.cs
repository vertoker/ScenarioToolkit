using System.Collections.Generic;
using System.Linq;
using ScenarioToolkit.Shared.Extensions;
using UnityEngine;

namespace ScenarioToolkit.Core.Scriptables
{
    [CreateAssetMenu(fileName = nameof(ScenarioModules), menuName = "Scenario/" + nameof(ScenarioModules))]
    public class ScenarioModules : ScriptableObject
    {
        [SerializeField] private ScenarioModule[] modules;
        
        public IReadOnlyList<ScenarioModule> Modules => modules;
        public IEnumerable<ScenarioModule> ValidModules 
            => modules.Where(module => module); // not null
        
        public ScenarioModule First(string scenario)
            => ValidModules.First(module => module.ModuleIdentifier == scenario);
        public ScenarioModule FirstOrDefault(string scenario)
            => ValidModules.FirstOrDefault(module => module.ModuleIdentifier == scenario);
        public ScenarioModule First(TextAsset asset)
            => ValidModules.First(module => module.ScenarioAsset == asset);
        public ScenarioModule FirstOrDefault(TextAsset asset)
            => ValidModules.FirstOrDefault(module => module.ScenarioAsset == asset);
        
        public void AddFromResources()
        {
            var set = modules.ToHashSet();
            set.AddRange(FindScriptables<ScenarioModule>("Assets/Resources"));
            modules = set.ToArray();
        }
        public void AddFromConfigs()
        {
            var set = modules.ToHashSet();
            set.AddRange(FindScriptables<ScenarioModule>("Assets/Configs"));
            modules = set.ToArray();
        }
        public void AddFromAssets()
        {
            var set = modules.ToHashSet();
            set.AddRange(FindScriptables<ScenarioModule>("Assets"));
            modules = set.ToArray();
        }
        
        private static IEnumerable<TAsset> FindScriptables<TAsset>(string folder) where TAsset : ScriptableObject
        {
#if UNITY_EDITOR
            var guids = UnityEditor.AssetDatabase.FindAssets($"t:{typeof(TAsset).Name}", new []{ folder });
            var paths = guids.Select(UnityEditor.AssetDatabase.GUIDToAssetPath);
            var assets = paths.Select(UnityEditor.AssetDatabase.LoadAssetAtPath<TAsset>);
            return assets;
#else
            throw new NotImplementedException("Use only in Editor");
#endif
        }
    }
}