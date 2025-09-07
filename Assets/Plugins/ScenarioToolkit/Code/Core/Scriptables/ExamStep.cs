using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace ScenarioToolkit.Core.Scriptables
{
    [CreateAssetMenu(fileName = nameof(ExamStep), menuName = "Scenario/" + nameof(ExamStep))]
    public class ExamStep : ScriptableObject
    {
        [SerializeField] private string stepIdentifier;
        [TextArea] [SerializeField] private string stepText;
        [SerializeField] private float time;
        [Space]
        [SerializeField] private SerializedDictionary<string, string> additionalData;

        public string StepIdentifier => stepIdentifier;
        public string StepText => stepText;
        public float Time => time;
        
        public SerializedDictionary<string, string> AdditionalData => additionalData;
    }
}