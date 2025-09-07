// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model.Interfaces
{
    /// <summary> При создании компонента в сценарии, устанавливает стандартные значения компоненту </summary>
    public interface IComponentDefaultValues
    {
        void SetDefault();
    }
}