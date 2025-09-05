
// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
    /// <summary>
    /// Способ активации ноды в плеере. По умолчанию все ноды имеют поведение AND
    /// </summary>
    public enum ActivationType : byte
    {
        /// <summary> Нода активируется только если все предыдущие ноды выполнятся </summary>
        AND = 1,
        /// <summary> Нода активируется если хотя бы одна нода предыдущая выполнится </summary>
        OR = 2,
    }
    
    // Написано в UPPERCASE для аналогии с бинарными операциями
}