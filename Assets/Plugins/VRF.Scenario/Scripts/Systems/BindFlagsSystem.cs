using Scenario.Core;
using Scenario.Core.Systems;
using VRF.DataSources.LocalCache;
using VRF.Scenario.Components.Actions;
using VRF.Scenario.UI.Exam.Report;
using Zenject;

namespace VRF.Scenario.Systems
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class BindFlagsSystem : BaseScenarioSystem
    {
        private readonly LocalCacheDataSource dataSource;

        public BindFlagsSystem(LocalCacheDataSource dataSource, SignalBus bus) : base(bus)
        {
            this.dataSource = dataSource;
            bus.Subscribe<BindExamStatisticsFlag>(BindExamStatisticsFlag<ExamStatisticsModelFlag>);
        }

        private void BindExamStatisticsFlag<TData>(BindExamStatisticsFlag component) where TData : class, new()
        {
            dataSource.Save(new TData());
        }
    }
}