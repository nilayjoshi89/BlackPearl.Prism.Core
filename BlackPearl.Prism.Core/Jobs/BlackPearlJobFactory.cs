using System;

using Quartz;
using Quartz.Simpl;
using Quartz.Spi;

using Unity;

namespace BlackPearl.Prism.Core.Jobs
{
    internal class BlackPearlJobFactory : PropertySettingJobFactory
    {
        private readonly IUnityContainer container;

        public BlackPearlJobFactory(IUnityContainer container)
        {
            this.container = container;
        }
        protected override IJob InstantiateJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            Type jobCoreType = bundle.JobDetail.JobType;
            var jobInstance = container.Resolve(jobCoreType) as IJob;

            string key = bundle.Trigger.JobDataMap.GetString(BlackPearlScheduler.UniqueSchedulingKey);
            var executorType = bundle.Trigger.JobDataMap.Get(BlackPearlScheduler.JobType) as Type;
            object executoryInstance = container.Resolve(executorType, key);

            jobCoreType.GetProperty(nameof(JobCore<object>.JobToExecute))?.SetValue(jobInstance, executoryInstance);

            return jobInstance;
        }
    }
}
