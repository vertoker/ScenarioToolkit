namespace VRF.DataSources.Config
{
    /// <summary>
    /// Список универасальных папок для сохранения файлов,
    /// их универсальность заключается в том, что они работают как
    /// и в Editor, так и в Runtime
    /// </summary>
    public enum ConfigFolder
    {
        StreamingAssets = 0,
        StreamingAssetsConfigs = 1,
        PersistentData = 2,
        // TODO добавить больше режимов
    }
}