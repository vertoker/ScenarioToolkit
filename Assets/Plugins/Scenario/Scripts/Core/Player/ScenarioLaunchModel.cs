using System;
using System.Text;
using NaughtyAttributes;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Serialization;
using VRF.Players.Scriptables;

// ReSharper disable once CheckNamespace
namespace Scenario.Core.DataSource
{
    [Serializable]
    public class ScenarioLaunchModel
    {
        [JsonProperty(nameof(scenario))]
        [SerializeField] private string scenario = string.Empty;
        [JsonProperty(nameof(useNetwork))]
        [SerializeField] private bool useNetwork = true;
        [JsonProperty(nameof(useLog))]
        [SerializeField] private bool useLog = false;
        [JsonProperty(nameof(identityHash))]
        [SerializeField] private int identityHash = 0;

        [JsonIgnore] public string Scenario { get => scenario; set => scenario = value; }
        [JsonIgnore] public bool UseNetwork { get => useNetwork; set => useNetwork = value; }
        [JsonIgnore] public bool UseLog { get => useLog; set => useLog = value; }
        [JsonIgnore] public int IdentityHash { get => identityHash; set => identityHash = value; }

        public bool ValidScenario => string.IsNullOrEmpty(scenario);
        public bool ValidHash => identityHash != 0;

        public string GetStatusString()
        {
            var builder = new StringBuilder();
            
            builder.Append(ValidScenario
                ? "identifier:<b>anonymous</b>"
                : $"identifier:<b>{scenario}</b>");
            
            builder.Append($" useNet:<b>{useNetwork}</b>");
            if (ValidHash) builder.Append($" identityHash:<b>{identityHash}</b>");
            builder.Append($" useLog:<b>{useLog}</b>");

            return builder.ToString();
        }
    }
}