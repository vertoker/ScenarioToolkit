using System;
using VRF.DataSources.Interfaces;

namespace VRF.DataSources.Scriptables
{
    /// <summary>
    /// Интерфейс для Scriptable объекта, получает модель и его тип
    /// </summary>
    public interface IScriptableModel : IModelHandler
    {
        // Разделение интерфейсов имеет смысл, так как он используется 
        // как экземпляр scriptable объекта и использование generic в нём
        // не позволит им так гибко аперировать
        
        public object GetModelObject();
    }
}