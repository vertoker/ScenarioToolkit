using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace ScenarioToolkit.Editor.Utilities.Providers
{
    /// <summary>
    /// Удобный shortcut для загрузки файлов разметки. Имеет predefined пути для папок с ними,
    /// поэтому если и менять папку с разметкой, то и класс этот лучше переписать на нечто динамическое
    /// </summary>
    [CreateAssetMenu(fileName = nameof(UxmlEditorProvider), 
        menuName = "Scenario Toolkit/Editor/" + nameof(UxmlEditorProvider))]
    public class UxmlEditorProvider : ScriptableSingleton<UxmlEditorProvider>
    {
        [field: Header("Context")]
        [field: SerializeField] public VisualTreeAsset ContextActiveEditor { get; private set; }
        [field: SerializeField] public VisualTreeAsset ContextEmptyEditor { get; private set; }
        
        [field: Header("Element")]
        [field: SerializeField] public VisualTreeAsset ElementGroupEditor { get; private set; }
        [field: SerializeField] public VisualTreeAsset ElementLinkEditor { get; private set; }
        [field: SerializeField] public VisualTreeAsset ElementNodeEditor { get; private set; }
        
        [field: Header("Fields")]
        [field: SerializeField] public VisualTreeAsset FieldsObjectTyped { get; private set; }
        [field: SerializeField] public VisualTreeAsset FieldsVariable { get; private set; }
        
        [field: Header("Lists")]
        [field: SerializeField] public VisualTreeAsset ListsComponents { get; private set; }
        [field: SerializeField] public VisualTreeAsset ListsVariables { get; private set; }
        
        [field: Header("Nodes")]
        [field: SerializeField] public VisualTreeAsset NodesComponent { get; private set; }
        [field: SerializeField] public VisualTreeAsset NodesField { get; private set; }
        [field: SerializeField] public VisualTreeAsset NodesSubgraph { get; private set; }
        
        [field: Header("SRF")]
        [field: SerializeField] public VisualTreeAsset SrfField { get; private set; }
        [field: SerializeField] public VisualTreeAsset SrfFunc { get; private set; }
        [field: SerializeField] public VisualTreeAsset SrfWindow { get; private set; }
        
        [field: Space]
        [field: SerializeField] public VisualTreeAsset GraphEditor { get; private set; }
        [field: SerializeField] public VisualTreeAsset SearchField { get; private set; }
        
        
        
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