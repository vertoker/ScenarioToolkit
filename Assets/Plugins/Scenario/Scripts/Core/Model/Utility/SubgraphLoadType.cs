
// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
    /// <summary>
    /// Способ загрузки данных для сабграфа. По умолчанию TextAsset
    /// </summary>
    public enum SubgraphLoadType : byte
    {
        /// <summary> Использует Resources, чтобы загрузить TextAsset со сценарием </summary>
        TextAsset = 1,
        /// <summary> Локальный путь на файл в папке StreamingAssets/ </summary>
        StreamingAsset = 2,
        /// <summary> Абсолютный путь на файл (только на Windows) </summary>
        AbsoluteAsset = 3,
    }
}