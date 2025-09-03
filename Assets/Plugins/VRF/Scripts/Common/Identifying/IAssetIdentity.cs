namespace VRF.Utils.Identifying
{
    /// <summary>
    /// Кастомные идентификаторы для ассетов, которые работают как в Editor, так и в Runtime.
    /// Гораздо важнее, что AssetHashCode неизменен для всех любых объектов, отчего поиск
    /// по этому индексу значительно упрощается
    /// </summary>
    public interface IAssetIdentity
    {
        /// <summary>
        /// Основной идентификатор, задаётся в ассетах и не меняется при спауне
        /// </summary>
        public int AssetHashCode { get; }
        /// <summary>
        /// Дополнительный идентификатор, использует Object.GetHashCode(), меняется при спауне
        /// </summary>
        public int RuntimeHashCode { get; }
    }
}