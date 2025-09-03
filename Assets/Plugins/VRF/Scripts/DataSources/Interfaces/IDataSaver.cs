namespace VRF.DataSources.Interfaces
{
    /// <summary>
    /// Сохранение модели в данные
    /// </summary>
    public interface IDataSaver
    {
        public bool Save<TData>(TData data);
    }
}