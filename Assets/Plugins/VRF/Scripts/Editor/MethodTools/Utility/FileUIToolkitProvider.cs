using System;
using System.IO;
using UnityEditor;
using UnityEngine.UIElements;

namespace VRF.Editor.MethodTools.Utility
{
    public class FileUIToolkitProvider
    {
        private readonly string _folderUI;
        private readonly string _uxmlFolder;
        private readonly string _ussFolder;

        public FileUIToolkitProvider(string folderUI, string uxmlFolder = "UXML", string ussFolder = "USS")
        {
            _folderUI = folderUI;
            _uxmlFolder = uxmlFolder;
            _ussFolder = ussFolder;
        }

        public VisualTreeAsset GetTree(string name)
        {
            var path = Path.Combine(_folderUI, _uxmlFolder, $"{name}.uxml");
            var tree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(path);
            if (!tree) throw new NullReferenceException($"Don't found UXML file at path {path}");
            return tree;
        }
        public StyleSheet GetStyleSheet(string name)
        {
            var path = Path.Combine(_folderUI, _ussFolder, $"{name}.uss");
            var style = AssetDatabase.LoadAssetAtPath<StyleSheet>(path);
            if (!style) throw new NullReferenceException($"Don't found USS file at path {path}");
            return style;
        }
    }
}