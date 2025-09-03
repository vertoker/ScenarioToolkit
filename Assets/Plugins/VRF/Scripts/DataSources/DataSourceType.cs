namespace VRF.DataSources
{
    /// <summary>
    /// Источники данных, откуда/куда могут загрузиться/сохраниться модели данных
    /// </summary>
    public enum DataSourceType
    {
        /// <summary> Текстовые файлы внутри универсальных папок через Newtonsoft JSON </summary>
        ConfigParser = 0,
        /// <summary> Кастомный парсер для аргументов командной строки </summary>
        CommandLine = 1,
        /// <summary> Scriptable объекты внутри самого Unity </summary>
        Scriptable = 2,
        /// <summary> Локальный кэш, если нужно удобно из игры задать модель (например данные из меню) </summary>
        LocalCache = 3,
        /// <summary> Другой способ, которого изначально нет в стандартной библиотеке source объектов </summary>
        Custom = 11,
    }
}