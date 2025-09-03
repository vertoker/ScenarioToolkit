using System;
using UnityEditor;
using UnityEngine.UIElements;

namespace Scenario.Editor.Utilities.Providers
{
    /// <summary>
    /// Удобный shortcut для загрузки файлов разметки. Имеет predefined пути для папок с ними,
    /// поэтому если и менять папку с разметкой, то и класс этот лучше переписать на нечто динамическое
    /// </summary>
    public static class UIProvider
    {
        // TODO это стоит сделать динамическими путями с зависимости от названия и расположения модуля
        private const string UxmlRootPath = "Assets/Modules/Scenario/UIToolkit/UXML";
        private const string UssRootPath = "Assets/Modules/Scenario/UIToolkit/USS";

        public static VisualTreeAsset GetUxmlTree(string name)
        {
            var tree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>($"{UxmlRootPath}/{name}.uxml");
            if (!tree) throw new NullReferenceException($"{name} not found in UXML/ folder");
            return tree;
        }
        public static StyleSheet GetUssSheet(string name)
        {
            var style = AssetDatabase.LoadAssetAtPath<StyleSheet>($"{UssRootPath}/{name}.uss");
            if (!style) throw new NullReferenceException($"{name} not found in USS/ folder");
            return style;
        }
    }
}