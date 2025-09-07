using Scenario.Editor.Utilities;
using Scenario.Editor.Windows.ContextEditor;
using Scenario.Editor.Windows.ElementEditor;
using Scenario.Editor.Windows.GraphEditor;

namespace Scenario.Editor.Windows
{
    /// <summary>
    /// Scenario Window Editors Context - ссылки на все открытые сценарные окна (основные).
    /// Это Singleton, ссылки изменяются самими окнами, если ссылки null - они принудительно вызываются
    /// </summary>
    public static class SWEContext
    {
        // Singleton - вынужденная мера, так как Unity API Для EditorWindow очень плохо масштабируется,
        // а код редактора уже написан и сильно зависимо от окон. Это можно исправить, но для этого
        // нужно переписывать окна и вынести основную логику куда-то вне Unity окон
        // Кандидаты
        // - GameObject - если решить проблему с мульти-сценами, может подойти
        // - ScriptableObject - хороший вариант, самый вероятный 
        
        private static GraphEditorWindow GraphImpl = null;
        private static ElementEditorWindow ElementImpl = null;
        private static ContextEditorWindow ContextImpl = null;
        
        public static GraphEditorWindow Graph
        {
            get
            {
                if (!GraphImpl)
                {
                    //Debug.Log("Construct Graph");
                    GraphImpl = WindowsStatic.GetGraphEditor(false);
                }
                return GraphImpl;
            }
        }
        public static ElementEditorWindow Element
        {
            get
            {
                if (!ElementImpl)
                {
                    //Debug.Log("Construct Element");
                    ElementImpl = GraphEditorWindow.ConstructElementEditor();
                }
                return ElementImpl;
            }
        }
        public static ContextEditorWindow Context
        {
            get
            {
                if (!ContextImpl)
                {
                    //Debug.Log("Construct Context");
                    ContextImpl = GraphEditorWindow.ConstructContextEditor();
                }
                return ContextImpl;
            }
        }

        internal static void SetGraph(GraphEditorWindow graph) => GraphImpl = graph;
        internal static void SetElement(ElementEditorWindow element) => ElementImpl = element;
        internal static void SetContext(ContextEditorWindow context) => ContextImpl = context;
    }
}