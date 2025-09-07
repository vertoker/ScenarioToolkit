#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ScenarioToolkit.Shared.Attributes
{
    // Taken from CreativeMode
    
    // v 1.1
    [CustomPropertyDrawer(typeof(SceneReferenceAttribute))]
    public class SceneReferenceDrawer : PropertyDrawer
    {
        private const string MissingTag = "[Missing] ";

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.String)
            {
                var value = property.stringValue;
                var missing = false;
                var empty = false;
                
                if (value.StartsWith(MissingTag))
                {
                    value = value.Replace(MissingTag, string.Empty);
                    missing = true;
                }
                else if (string.IsNullOrWhiteSpace(value))
                {
                    empty = true;
                }
                
                var sceneObject = TryLoadSceneAsset(value);
                
                // Здесь мы ожидаем, что сцена будет найдена
                // Если путь empty, то ссылка просто пустая и ошибка просто не нужна
                // Если путь missing, то ссылка уже идентифицирована как missing и ошибка вывелась до этого
                // Если флаги не стоят и объект null, то точно выводим ошибку
                if (!sceneObject && !empty && !missing)
                {
                    Debug.LogError($"Could not find scene {value} in {property.propertyPath}");
                    missing = true;
                }
                
                var newSceneAsset = (SceneAsset)EditorGUI.ObjectField(position, label, sceneObject, typeof(SceneAsset), true);
                
                var newScenePath = AssetDatabase.GetAssetPath(newSceneAsset);
                // Если путь будет найден, то просто записываем путь до найденной сцены
                if (!string.IsNullOrWhiteSpace(newScenePath))
                {
                    value = newScenePath;
                    missing = false;
                }

                if (missing) value = MissingTag + value;
                property.stringValue = value;
            }
            else
            {
                EditorGUI.LabelField(position, label.text, "Use [SceneReference] with strings");
            }
        }

        private static SceneAsset TryLoadSceneAsset(string scenePath)
        {
            var sceneObject = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);
            if (sceneObject) return sceneObject;

            sceneObject = GetBuildSettingsSceneObject(scenePath);
            if (sceneObject) return sceneObject;
            
            return null;
        }

        private static SceneAsset GetBuildSettingsSceneObject(string sceneName)
        {
            return EditorBuildSettings.scenes
                .Select(buildScene => AssetDatabase.LoadAssetAtPath<SceneAsset>(buildScene.path))
                .FirstOrDefault(sceneAsset => sceneAsset && sceneAsset.name == sceneName);
        }
    }
}
#endif