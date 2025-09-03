// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model.Interfaces
{
    /// <summary>
    /// Интерфейс для объекта, который должен иметь хэш
    /// </summary>
    public interface IHashableSource : IHashable
    {
        public new int Hash { get; set; }

        /// <summary> GetBaseHashCode - настоящий GetHashCode </summary>
        public int GetBaseHashCode();
    }
}