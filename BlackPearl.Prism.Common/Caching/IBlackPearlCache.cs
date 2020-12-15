using System;
using System.Threading.Tasks;

namespace BlackPearl.Prism.Common.Caching
{
    public interface IBlackPearlCache
    {
        Task Initialize(string dbLocation);
        Task<T> Get<T>(string key);
        Task<T> GetOrCreate<T>(string key, Func<T> createAction, TimeSpan? expireTimeSpan);
        Task Set<T>(string key, T value, TimeSpan? expireTimeSpan);
        Task Remove(string key);
        Task RemoveAll();
    }
}
