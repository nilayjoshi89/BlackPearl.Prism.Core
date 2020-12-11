using System.Linq;
using System.Threading.Tasks;

using BlackPearl.Prism.Jobs;

using Quartz;

namespace BlackPearl.Prism.Core.Jobs
{
    internal sealed class JobCore<T> : IJob
        where T : class, new()
    {
        private readonly IJobDataMapper mapper;

        public JobCore(IJobDataMapper mapper)
        {
            this.mapper = mapper;
        }

        public IBlackPearlJob<T> JobToExecute { get; set; }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                T data = (context.MergedJobDataMap?.Keys?.Any() == true)
                        ? mapper.Map<T>(context.MergedJobDataMap)
                        : default(T);

                await JobToExecute.Execute(data);
            }
            catch { }
        }
    }
}
