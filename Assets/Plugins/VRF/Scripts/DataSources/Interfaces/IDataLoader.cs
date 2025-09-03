namespace VRF.DataSources.Interfaces
{
    /// <summary>
    /// Загрузка данных в модель
    /// </summary>
    public interface IDataLoader
    {
        public TData Load<TData>() where TData : class;
    }
}