using System;
using Scenario.Utilities;

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model.Interfaces
{
    /// <summary>
    /// Базовый класс для всех интерфейсов моделей, предоставляет недостающий функционал
    /// для работы с классами через интерфейсы
    /// </summary>
    /// <typeparam name="TModel">Тип последней актуальной модели</typeparam>
    /// <typeparam name="TInterfaceModel">Дочерний тип интерфейса</typeparam>
    public interface IModelReflection<TModel, TInterfaceModel> where TModel : TInterfaceModel
    {
        /// <summary> Отдаёт актуальный тип модели </summary>
        public static Type GetModelType => typeof(TModel);
        
        /// <summary> Создаёт экземпляр модели исключительно через интерфейс </summary>
        public static TInterfaceModel CreateNew()
        {
            var node = (TInterfaceModel)Activator.CreateInstance(GetModelType);
            if (node is IHashableSource hashable) hashable.InitializeHash();
            return node;
        }
    }
}