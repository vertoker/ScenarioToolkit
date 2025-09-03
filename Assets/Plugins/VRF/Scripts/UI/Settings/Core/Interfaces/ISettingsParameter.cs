namespace VRF.UI.Settings.Core.Interfaces
{
    /// <summary>
    /// Интерфейс для связи основного контроллера настроек и его параметров
    /// </summary>
    public interface ISettingsParameter
    {
        /// <summary>
        /// Первичная загрузка модели в параметры
        /// </summary>
        /// <param name="model">Readonly версия модели</param>
        public void SetupModel(IReadOnlySettingsModel model);
    }
}