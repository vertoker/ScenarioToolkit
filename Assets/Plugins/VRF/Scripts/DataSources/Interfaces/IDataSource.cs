namespace VRF.DataSources.Interfaces
{
    /// <summary>
    /// Основной интерфейс, который нужен для имплементации
    /// в основной контейнер источников
    /// </summary>
    public interface IDataSource
    {
        public DataSourceType GetSelfType();
    }
}