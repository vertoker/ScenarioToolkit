using System;
using System.Collections.Generic;
using System.Linq;
using Scenario.Core;
using ScenarioToolkit.Core.World;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

// ReSharper disable once CheckNamespace
namespace Scenario.Utilities
{
    [ExecuteAlways]
    public class MultiEnabler : MonoBehaviour
    {
        [SerializeField] private List<MonoBehaviour> monoBehaviours = new();

        public List<MonoBehaviour> MonoBehaviours
        {
            get => monoBehaviours;
            set => monoBehaviours = value;
        }

        private void OnEnable()
        {
            foreach (var mb in monoBehaviours)
            {
                if (!mb)
                    Debug.LogError("Null or destroyed mono behaviour", gameObject);
                mb.enabled = true;
            }
        }

        private void OnDisable()
        {
            if (enabled)
                return;

            foreach (var mb in monoBehaviours)
            {
                if (!mb)
                    Debug.LogError("Null or destroyed mono behaviour", gameObject);
                mb.enabled = false;
            }
        }

        public void Toggle() => enabled = !enabled;

        public void AddOwnMonoBehaviors()
        {
            monoBehaviours.Clear();
            AddFromGO(gameObject);
        }

        public void MonoOthersMonoBehaviors()
        {
            var oldMonos = new List<MonoBehaviour>(monoBehaviours);
            monoBehaviours.Clear();
            foreach (var mono in oldMonos)
                AddFromGO(mono.gameObject);
        }

        public void OthersMultiEnablers()
        {
            var oldMonos = new List<MonoBehaviour>(monoBehaviours);
            monoBehaviours.Clear();
            foreach (var mono in oldMonos)
                AddMultiEnablerFromGO(mono.gameObject);
        }

        private void AddFromGO(GameObject go)
        {
            var allMonoBehaviours = go.GetComponents<MonoBehaviour>();
            foreach (var mono in allMonoBehaviours)
            {
                if (mono is ScenarioBehaviour
                    || mono is MultiEnabler
                    || mono.GetType().Name.Contains("TargetStateListener") //TODO: Find a better way
                    || monoBehaviours.Contains(mono))
                    continue;
                monoBehaviours.Add(mono);
            }
        }

        private void AddMultiEnablerFromGO(GameObject go)
        {
            var allMultiEnablers = go.GetComponents<MultiEnabler>();
            foreach (var multiEnabler in allMultiEnablers)
                if (!monoBehaviours.Contains(multiEnabler))
                    monoBehaviours.Add(multiEnabler);
        }
        
#if UNITY_EDITOR
        [CustomEditor(typeof(MultiEnabler)), CanEditMultipleObjects]
        public class MultiEnablerEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                DrawDefaultInspector();

                if (GUILayout.Button("Toggle"))
                {
                    targets.OfType<MultiEnabler>().ToList().ForEach(multiEnabler => multiEnabler.Toggle());
                }
                if (GUILayout.Button("Add Own MonoBehaviors"))
                {
                    targets.OfType<MultiEnabler>().ToList().ForEach(multiEnabler => multiEnabler.AddOwnMonoBehaviors());
                }
                if (GUILayout.Button("Add MonoBehaviors for all existing objects"))
                {
                    targets.OfType<MultiEnabler>().ToList()
                        .ForEach(multiEnabler => multiEnabler.MonoOthersMonoBehaviors());
                }
                if (GUILayout.Button("Add MultiEnabler for all existing objects"))
                {
                    targets.OfType<MultiEnabler>().ToList().ForEach(multiEnabler => multiEnabler.OthersMultiEnablers());
                }
            }
        }
#endif
        
    }
}