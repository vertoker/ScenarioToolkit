using System;
using UnityEngine;

namespace VRF.DataSources.Scriptables.Base
{
    /// <summary>
    /// Стандартная имплементация интерфейса IScriptableModel,
    /// используется если модель можно без проблем сериализовать в самом Unity
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public abstract class BaseScriptableModel<TModel> : ScriptableObject, IScriptableModel
    {
        [SerializeField] private TModel model;
        
        public Type GetModelType() => typeof(TModel);
        object IScriptableModel.GetModelObject() => model;
        public void SetModel(TModel newModel) => model = newModel;
        public TModel GetModel() => model;
    }
}