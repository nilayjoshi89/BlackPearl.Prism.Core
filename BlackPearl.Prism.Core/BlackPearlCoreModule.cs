
using System.Threading.Tasks;

using BlackPearl.Prism.Common.Caching;
using BlackPearl.Prism.Core.Caching;
using BlackPearl.Prism.Core.Jobs;
using BlackPearl.Prism.Jobs;

using Prism.Ioc;
using Prism.Modularity;

using Quartz.Spi;

namespace BlackPearl.Prism.Core
{
    public sealed class BlackPearlCoreModule : IModule
    {
        public async void OnInitialized(IContainerProvider containerProvider)
        {
            try
            {
                IBlackPearlScheduler scheduler = containerProvider.Resolve<IBlackPearlScheduler>();
                IBlackPearlCache cache = containerProvider.Resolve<IBlackPearlCache>();

                Task scheduleTask = scheduler.Initialize();
                Task cacheTask = cache.Initialize("BlackPearlCoreDataStore.db");

                await Task.WhenAll(scheduleTask, cacheTask);
            }
            catch { }
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IJobFactory, BlackPearlJobFactory>();
            containerRegistry.Register<IJobDataMapper, IJobDataMapper>();
            containerRegistry.RegisterSingleton<IBlackPearlScheduler, BlackPearlScheduler>();
            containerRegistry.RegisterSingleton<IBlackPearlCache, BlackPearlCache>();
        }
    }
}
