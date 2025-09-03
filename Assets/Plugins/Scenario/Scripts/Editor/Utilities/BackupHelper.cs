using System.IO;
using System.Linq;

namespace Scenario.Editor.Utilities
{
    public static class BackupHelper
    {
        public static void SaveBackup(string data, string name)
        {
            File.WriteAllText(Path.Join(DirectoryFileHelper.BackupDirectory, DirectoryFileHelper.GetNewFileName($"backup-{name}")), data);
        }

        public static string LoadLastBackup()
        {
            var info = new DirectoryInfo(DirectoryFileHelper.BackupDirectory);
            var path = info.GetFiles()
                .Where(file => file.Name.ToLowerInvariant().EndsWith("json"))
                .OrderBy(file => file.CreationTime).LastOrDefault()?.FullName;
            return string.IsNullOrEmpty(path) ? string.Empty : File.ReadAllText(path);
        }
    }
}