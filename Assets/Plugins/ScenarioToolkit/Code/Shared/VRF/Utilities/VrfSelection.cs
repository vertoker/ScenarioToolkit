using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

#if UNITY_EDITOR

#else
using VRF.Utilities.Exceptions;
#endif

namespace VRF.Utilities
{
    /// <summary>
    /// Класс с функционалом по работе с выбранными объектами в эдиторе (получение объектов и их компонентов)
    /// </summary>
    public static class VrfSelection
    {
        public static GameObject ActiveContext
        {
            get
            {
#if UNITY_EDITOR
                return Selection.activeGameObject;
#else
                throw new OnlyUnityEditorException();
#endif
            }
        }
        public static Transform GetActiveContextTransform
        {
            get
            {
#if UNITY_EDITOR
                return Selection.activeTransform;
#else
                throw new OnlyUnityEditorException();
#endif
            }
        }
        
        public static Object[] SelectionObjects
        {
            get
            {
#if UNITY_EDITOR
                return Selection.objects;
#else
                throw new OnlyUnityEditorException();
#endif
            }
        }
        public static void SetSelection(params Object[] objects)
        {
#if UNITY_EDITOR
            Selection.objects = objects;
#else
            throw new OnlyUnityEditorException();
#endif
        }
        
        public static GameObject[] SelectionGameObjects
        {
            get
            {
#if UNITY_EDITOR
                return Selection.gameObjects;
#else
                throw new OnlyUnityEditorException();
#endif
            }
        }
        
        public static Transform[] SelectionTransforms
        {
            get
            {
#if UNITY_EDITOR
                return Selection.transforms;
#else
                throw new OnlyUnityEditorException();
#endif
            }
        }

        /// <summary>
        /// Получить все GameObject на активной сцене
        /// </summary>
        public static List<GameObject> SceneGameObjects
        {
            get
            {
                var scene = SceneManager.GetActiveScene();
                var list = new List<GameObject>(scene.rootCount);
                scene.GetRootGameObjects(list);
                return list;
            }
        }

        /// <summary>
        /// Получить все Transform на активной сцене
        /// </summary>
        public static IEnumerable<Transform> SceneTransforms => 
            SceneGameObjects.Select(obj => obj.transform);

    }
}