using JetBrains.Annotations;
using NaughtyAttributes;
using Scenario.Core.DataSource;
using ScenarioToolkit.Core.DataSource;
using ScenarioToolkit.Shared.Attributes;
using ScenarioToolkit.Shared.VRF;
using UnityEngine;

namespace ScenarioToolkit.Core.Scriptables
{
    [CreateAssetMenu(fileName = nameof(ScenarioModule), menuName = "Scenario/" + nameof(ScenarioModule))]
    public class ScenarioModule : ScriptableObject
    {
        [Header("Data")]
        [SerializeField] private string moduleIdentifier;
        [SerializeField] private TextAsset scenarioAsset;
        [SerializeField] private ScenarioMode mode = ScenarioMode.Study;
        [SceneReference, SerializeField] private string scene;
        
        [Header("Properties")]
        [SerializeField] private string moduleName;
        [TextArea(3, 15)] [SerializeField] private string moduleDescription;
        [SerializeField, Expandable] private ExamStep[] examSteps;
        
        public string ModuleIdentifier => moduleIdentifier;
        public bool IsValidIdentifier() => !string.IsNullOrWhiteSpace(moduleIdentifier);
        public TextAsset ScenarioAsset => scenarioAsset;
        public ScenarioMode Mode => mode;
        public string ScenePath => scene;
        
        public string ModuleName => moduleName;
        public string ModuleDescription => moduleDescription;
        public ExamStep[] ExamSteps => examSteps;
        
        public void SetIdentifier(string newIdentifier) => moduleIdentifier = newIdentifier;
        public void SetScenario(TextAsset newAsset) => scenarioAsset = newAsset;
        public void SetMode(ScenarioMode newMode) => mode = newMode;
        public void SetScene(string newScene) => scene = newScene;

        public ScenarioLaunchModel GetModel([CanBeNull] PlayerIdentityConfig identity)
        {
            var identityHash = identity ? identity.AssetHashCode : 0;
            var model = new ScenarioLaunchModel
            {
                Scenario = ModuleIdentifier,
                IdentityHash = identityHash,
            };
                    
            Debug.Log($"Load scenario={model.Scenario} " +
                      $"on scene={ScenePath}");
            return model;
        }
    }
}