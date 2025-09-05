using System;
using System.IO;
using VRF.Utilities;

namespace Scenario.Editor.Utilities
{
    public static class DirectoryFileHelper
    {
        public const string SaveDirectoryLocal = "Assets/Resources/Scenario/Jsons";
        public const string ModulesDirectoryLocal = "Assets/Resources/Scenario/Modules";
        public const string EditorDirectoryLocal = "ScenarioEditor";
        public const string BackupDirectoryLocal = EditorDirectoryLocal + "/Backups";

        public static string SaveDirectory => GetValidatedPath(SaveDirectoryLocal);
        public static string ModulesDirectory => GetValidatedPath(ModulesDirectoryLocal);
        public static string EditorDirectory => GetValidatedPath(EditorDirectoryLocal);
        public static string BackupDirectory => GetValidatedPath(BackupDirectoryLocal);
        
        private static string GetValidatedPath(string projectPath)
        {
            var absolutePath = VrfPath.FromProjectToAbsolute(projectPath);
            ValidateDirectory(absolutePath);
            return absolutePath;
        }
        public static void ValidateDirectory(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);
        }
        public static void InitGitignore()
        {
            var path = EditorDirectory + "/.gitignore";
            
            if (File.Exists(path)) return;
            
            const string contents = "/Backups/\n" + 
                                    "/session.json";
            
            File.WriteAllText(path, contents);
        }
        
        public static string GetNewFileName(string prefix)
        {
            var time = DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss");
            return $"{prefix}-{time}.json";
        }
        public static bool TryLoadFile(string absoluteFilePath, out string data)
        {
            if (!File.Exists(absoluteFilePath))
            {
                data = string.Empty;
                return false;
            }
            
            data = File.ReadAllText(absoluteFilePath);
            return true;
        }

        public static string GetFileName(string filePath, bool withExtension = false)
        {
            if (!File.Exists(filePath)) return string.Empty;
            var info = new FileInfo(filePath);
            return withExtension ? info.Name 
                : info.Name.Replace(info.Extension, string.Empty);
        }
        public static string GetFileDirectory(string filePath)
        {
            if (!File.Exists(filePath)) return string.Empty;
            var info = new FileInfo(filePath);
            return info.DirectoryName;
        }
    }
}