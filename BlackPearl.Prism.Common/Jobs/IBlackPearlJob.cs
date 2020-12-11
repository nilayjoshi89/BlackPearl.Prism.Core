using System.Threading.Tasks;

namespace BlackPearl.Prism.Jobs
{
    public interface IBlackPearlJob<T>
        where T : class, new()
    {
        Task Execute(T executionData);
    }
}
