namespace ScenarioToolkit.Editor.Content.Fields
{
    /// <summary>
    /// Все доступные поля для отрисовки в Unity, в основном это всё unity поля
    /// или их комбинация с другими UI элементами
    /// </summary>
    public enum ScenarioFieldType
    {
        // Core
        String     =   0,
        UObject    =   1,
        Int        =   2,
        Float      =   3,
        Enum       =   4,
        Bool       =   5,
        
        // Unity
        Vector2    =  10,
        Vector3    =  11,
        Vector4    =  12,
        Color      =  13,
        Vector2Int =  14,
        Vector3Int =  15,
        Bounds     =  16,
        Rect       =  17,
        BoundsInt  =  18,
        RectInt    =  19,
        Hash128    =  20,
        
        // C#
        Long       =  31,
        Double     =  32,
        UInt       =  33,
        ULong      =  34,
        
        // Custom Addressing
        NodeLink   =  51,
        
        // Convertible Fields
        Quaternion = 101,
    }
}