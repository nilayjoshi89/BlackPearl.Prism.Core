using System.Threading.Tasks;

namespace BlackPearl.Prism.Jobs
{
    public interface IBlackPearlScheduler
    {
        Task Initialize();
        Task ScheduleJob<T, D>(string key, string group, string cronString, D data)
            where T : IBlackPearlJob<D>
            where D : class, new();
        Task DeleteJob<T, D>(string key, string group)
            where T : IBlackPearlJob<D>
            where D : class, new();
        Task UpdateJob<T, D>(string key, string group, D data)
            where T : IBlackPearlJob<D>
            where D : class, new();
        Task ForceRun<T, D>(string key, string group)
            where T : IBlackPearlJob<D>
            where D : class, new();
    }
}
