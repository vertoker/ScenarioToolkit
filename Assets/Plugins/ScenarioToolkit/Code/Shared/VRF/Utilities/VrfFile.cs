using System.IO;
#if UNITY_EDITOR
#else
using VRF.Utilities.Exceptions;
#endif

namespace VRF.Utilities
{
    /// <summary>
    /// Класс для работы с файлами и папками
    /// </summary>
    public static class VrfFile
    {
        private const char IndexingBlock = '~';
        private const string IndexingBlockSlash = "~/";
        
        public static void SetUnityIndexingActiveForDirectory(string absolutePath, bool enableIndexing)
        {
            string indexPath, notIndexPath;
            
            if (absolutePath.EndsWith(IndexingBlock))
            {
                indexPath = absolutePath;
                notIndexPath = indexPath[..^1];
            }
            else if (absolutePath.EndsWith(IndexingBlockSlash))
            {
                indexPath = absolutePath;
                notIndexPath = indexPath[..^2];
            }
            else
            {
                notIndexPath = absolutePath;
                indexPath = notIndexPath + IndexingBlock;
            }
            
            if (Directory.Exists(notIndexPath))
                Directory.Move(notIndexPath, indexPath);
            else if (Directory.Exists(absolutePath))
                Directory.Move(indexPath, notIndexPath);
        }
        
        /// <summary>
        /// Копирует файлы из sourcePath в targetPath с сохранением структуры папок. При этом любые файлы, изначально находящиеся в целевой папке, удаляются.
        /// </summary>
        public static void CopyFiles(string sourcePath, string targetPath)
        {
            ValidateEmptyDirectory(targetPath);
            
            //Now Create all of the directories
            foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
            }

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(sourcePath, "*.*",SearchOption.AllDirectories))
            {
                File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
            }
        }

        /// <summary>
        /// Удаляет папку по заданному пути (работает только в эдиторе)
        /// </summary>
        public static void DeleteDirectoryEditor(string absolutePath)
        {
#if UNITY_EDITOR
            if (!Directory.Exists(absolutePath)) return;
            if (!absolutePath.EndsWith(Path.DirectorySeparatorChar))
                Directory.Delete(absolutePath + Path.DirectorySeparatorChar, true);
            else Directory.Delete(absolutePath, true);
            File.Delete(absolutePath + VrfAssets.MetaExtension);
#else
            throw new OnlyUnityEditorException();
#endif
        }
        
        /// <summary>
        /// Создает директорию по заданному пути, если ее не существует
        /// </summary>
        public static void ValidateDirectory(string absolutePath)
        {
            if (!Directory.Exists(absolutePath))
                Directory.CreateDirectory(absolutePath);
        }
        
        /// <summary>
        /// Удаляет все файлы в директории по заданному пути либо создает её
        /// </summary>
        public static void ValidateEmptyDirectory(string absolutePath)
        {
            if (Directory.Exists(absolutePath))
            {
                foreach (var filePath in Directory.GetFiles(absolutePath))
                    File.Delete(filePath);
            }
            else
            {
                Directory.CreateDirectory(absolutePath);
            }
        }
    }
}