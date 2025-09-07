using System;
using UnityEditor;
using UnityEngine;

namespace ScenarioToolkit.Editor.Utilities.Providers
{
    public static class PresetProvider
    {
        // TODO это стоит сделать динамическими путями с зависимости от названия и расположения модуля
        private const string PresetsPath = "Assets/Modules/Scenario/Editor/Presets";

        public static string GetPresetText(string name)
        {
            var textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>($"{PresetsPath}/{name}.json");
            if (!textAsset) throw new NullReferenceException($"{name} not found in Presets/ folder");
            return textAsset.text;
        }
    }
} 