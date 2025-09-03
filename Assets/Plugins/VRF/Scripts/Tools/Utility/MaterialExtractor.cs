#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace VRF.Tools.Utility
{
    public class MaterialExtractor : Editor
    {
        public static void ExtractMaterials(string assetPath, string destinationPath)
        {
            var hashSet = new HashSet<string>();
            var enumerable = from x in AssetDatabase.LoadAllAssetsAtPath(assetPath) where x.GetType() == typeof(Material) select x;
            foreach (var item in enumerable)
            {
                var path = Path.Combine(destinationPath, item.name) + ".mat";
                path = AssetDatabase.GenerateUniqueAssetPath(path);
                var value = AssetDatabase.ExtractAsset(item, path);
                if (string.IsNullOrEmpty(value))
                    hashSet.Add(assetPath);
            }
 
            foreach (var item2 in hashSet)
            {
                AssetDatabase.WriteImportSettingsIfDirty(item2);
                AssetDatabase.ImportAsset(item2, ImportAssetOptions.ForceUpdate);
            }
        }

        //[MenuItem(ToolsNamespace.UtilityExtractMaterialsFromFBX)]
        public static void ExtractAllFBX()
        {
            foreach (var obj in Selection.GetFiltered(typeof(Object), SelectionMode.Assets))
            {
                var path = AssetDatabase.GetAssetPath(obj);
                var folder = Path.GetDirectoryName(path);
                ExtractMaterials(path, folder);
            }
        }
    }
}
#endif
