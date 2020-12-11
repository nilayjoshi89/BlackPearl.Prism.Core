using System;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

using BlackPearl.Prism.Jobs;

using Quartz;
using Quartz.Impl;
using Quartz.Spi;

using Unity;

namespace BlackPearl.Prism.Core.Jobs
{
    internal sealed class BlackPearlScheduler : IBlackPearlScheduler
    {
        public const string JobType = "JobType";
        public const string UniqueSchedulingKey = "UniqueSchedulingKey";
        private readonly IJobDataMapper mapper;
        private readonly IJobFactory jobFactory;
        private readonly IUnityContainer container;
        private IScheduler scheduler;

        public BlackPearlScheduler(IJobDataMapper mapper, IJobFactory jobFactory, IUnityContainer container)
        {
            this.mapper = mapper;
            this.jobFactory = jobFactory;
            this.container = container;
        }

        public async Task DeleteJob<T, D>(string key, string group)
            where T : IBlackPearlJob<D>
            where D : class, new()
        {
            if (scheduler == null)
            {
                return;
            }

            var jobKey = new JobKey(key, group);
            bool jobFound = await scheduler.CheckExists(jobKey);

            if (!jobFound)
            {
                return;
            }

            await scheduler.DeleteJob(jobKey);
        }

        public async Task ForceRun<T, D>(string key, string group)
            where T : IBlackPearlJob<D>
            where D : class, new()
        {
            if (scheduler == null)
            {
                return;
            }

            var jobKey = new JobKey(key, group);
            bool jobExists = await scheduler.CheckExists(jobKey);

            if (!jobExists)
            {
                return;
            }

            System.Collections.Generic.IReadOnlyCollection<ITrigger> trigger = await scheduler.GetTriggersOfJob(jobKey);
            await scheduler.TriggerJob(jobKey, trigger?.FirstOrDefault()?.JobDataMap);
        }

        public async Task Initialize()
        {
            if (scheduler != null)
            {
                return;
            }

            var props = new NameValueCollection()
            {
                {"quartz.serializer.type","binary" }
            };
            var factory = new StdSchedulerFactory(props);
            scheduler = await factory.GetScheduler();
            scheduler.JobFactory = jobFactory;

            await scheduler.Start();
        }

        public async Task ScheduleJob<T, D>(string key, string group, string cronString, D data)
            where T : IBlackPearlJob<D>
            where D : class, new()
        {
            await CheckAndInitializeScheduler();

            string uniqueKey = Guid.NewGuid().ToString();
            container.RegisterType<T>(uniqueKey);

            IJobDetail jobDetail = JobBuilder.Create<JobCore<D>>()
                                            .WithIdentity(key, group)
                                            .Build();

            JobDataMap jobDataMap = GetJobDataMap<T, D>(data, uniqueKey);

            ITrigger trigger = TriggerBuilder.Create()
                                        .WithIdentity(key, group)
                                        .StartNow()
                                        .WithCronSchedule(cronString)
                                        .UsingJobData(jobDataMap)
                                        .ForJob(jobDetail)
                                        .Build();

            await scheduler.ScheduleJob(jobDetail, trigger);
        }

        public async Task UpdateJob<T, D>(string key, string group, D data)
            where T : IBlackPearlJob<D>
            where D : class, new()
        {
            if (scheduler == null)
            {
                return;
            }

            var jobKey = new JobKey(key, group);
            bool jobExists = await scheduler.CheckExists(jobKey);

            if (!jobExists)
            {
                return;
            }

            IJobDetail jobDetail = await scheduler.GetJobDetail(jobKey);
            mapper.UpdateMap(data, jobDetail.JobDataMap);
        }

        private async Task CheckAndInitializeScheduler()
        {
            if (scheduler == null)
            {
                await Initialize();
            }
        }
        private JobDataMap GetJobDataMap<T, D>(D data, string uniqueKey)
            where T : IBlackPearlJob<D>
            where D : class, new()
        {
            JobDataMap jobDataMap = (data != null)
                                        ? mapper.Map(data)
                                        : new JobDataMap();

            jobDataMap.Add(UniqueSchedulingKey, uniqueKey);
            jobDataMap.Add(JobType, typeof(T));
            return jobDataMap;
        }
    }
}
