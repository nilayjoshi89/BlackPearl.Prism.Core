
using BlackPearl.Prism.Core.Jobs;
using BlackPearl.Prism.Jobs;

using Prism.Ioc;
using Prism.Modularity;

using Quartz.Spi;

namespace BlackPearl.Prism.Core
{
    public sealed class BlackPearlCoreModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            try
            {
                var scheduler = containerProvider.Resolve<IBlackPearlScheduler>();
                scheduler.Initialize();
            }
            catch { }
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IJobFactory, BlackPearlJobFactory>();
            containerRegistry.Register<IJobDataMapper, IJobDataMapper>();
            containerRegistry.RegisterSingleton<IBlackPearlScheduler, BlackPearlScheduler>();
        }
    }
}
