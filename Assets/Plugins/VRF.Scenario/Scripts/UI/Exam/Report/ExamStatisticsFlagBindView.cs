using System;
using SimpleUI.Templates.Base;
using UnityEngine;
using UnityEngine.Events;
using VRF.DataSources.LocalCache;

namespace VRF.Scenario.UI.Exam.Report
{
    public class ExamStatisticsFlagBindView : BaseButtonView
    {
        [field:SerializeField] public bool Active { get; private set; } = true;
        
        public override Type GetControllerType() => typeof(ExamStatisticsFlagBindController);
    }
    public class ExamStatisticsFlagBindController : BaseButtonController<ExamStatisticsFlagBindView>
    {
        private readonly LocalCacheDataSource dataSource;

        public ExamStatisticsFlagBindController(LocalCacheDataSource dataSource, ExamStatisticsFlagBindView view) : base(view)
        {
            this.dataSource = dataSource;
        }

        protected override UnityAction GetAction() => Save;

        private void Save()
        {
            if (View.Active)
                dataSource.Save(new ExamStatisticsModelFlag());
        }
    }
    
    /// <summary>
    /// Флаг, который с помощью LocalCacheDataSource системы уведомляет о чём-то.
    /// Сам факт его существования в DataSource является переменной bool, которая используется другими системами в других сценах.
    /// Конкретно тут оно используется, чтобы вызывать окно статистики по экзамену и главном меню
    /// </summary>
    [Serializable] public class ExamStatisticsModelFlag { }
}