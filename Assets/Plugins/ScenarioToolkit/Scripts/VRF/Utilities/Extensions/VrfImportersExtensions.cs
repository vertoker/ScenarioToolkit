#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace VRF.Utilities.Extensions
{
    /// <summary>
    /// Расширения для работы с импортерами
    /// </summary>
    public static class VrfImportersExtensions
    {
        public static void ExtractMaterials(this ModelImporter importer, string absolutePath)
        {
            var projectPath = VrfPath.ConvertPath(absolutePath, PathRelative.Absolute, PathRelative.Project);
            
            try
            {
                AssetDatabase.StartAssetEditing();
                
                var stringSet = new HashSet<string>();
                foreach (var asset in AssetDatabase.LoadAllAssetsAtPath(importer.assetPath).Where(x => x.GetType() == typeof (Material)).ToArray())
                {
                    var uniqueAssetPath = AssetDatabase.GenerateUniqueAssetPath(string.Join(Path.DirectorySeparatorChar.ToString(), projectPath, asset.name) + ".mat");
                    if (string.IsNullOrEmpty(AssetDatabase.ExtractAsset(asset, uniqueAssetPath)))
                        stringSet.Add(importer.assetPath);
                }
            
                foreach (var path in stringSet)
                {
                    AssetDatabase.WriteImportSettingsIfDirty(path);
                    AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
                }
            }
            finally
            {
                AssetDatabase.StopAssetEditing();
            }
        }
    }
}
#endif