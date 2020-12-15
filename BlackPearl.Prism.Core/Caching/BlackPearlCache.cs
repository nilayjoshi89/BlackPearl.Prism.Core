using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;

using Akavache;
using Akavache.Sqlite3;

using BlackPearl.Prism.Common.Caching;

namespace BlackPearl.Prism.Core.Caching
{
    internal class BlackPearlCache : IBlackPearlCache, IDisposable
    {
        #region Members
        private IBlobCache cache;
        private bool disposedValue;
        #endregion

        #region Methods
        public Task Initialize(string dbLocation)
        {
            var t = Task.Run(() =>
            {
                cache = new SqlRawPersistentBlobCache(dbLocation)
                {
                    ForcedDateTimeKind = DateTimeKind.Utc
                };
            });
            t.ConfigureAwait(false);
            return t;
        }
        public Task<T> Get<T>(string key)
        {
            var t = Task.Run(async () =>
            {
                try
                {
                    return await cache.GetObject<T>(key);
                }
                catch (KeyNotFoundException)
                {
                    return default(T);
                }
            });
            t.ConfigureAwait(false);
            return t;
        }
        public Task<T> GetOrCreate<T>(string key, Func<T> createAction, TimeSpan? expireTimeSpan)
        {
            var t = Task.Run(async () =>
            {
                try
                {
                    DateTimeOffset? offset = expireTimeSpan.HasValue ? DateTimeOffset.UtcNow.Add(expireTimeSpan.Value) : (DateTimeOffset?)null;
                    return await cache.GetOrCreateObject(key, createAction, offset);
                }
                catch
                {
                    return default(T);
                }
            });
            t.ConfigureAwait(false);
            return t;
        }
        public Task Remove(string key)
        {
            var t = Task.Run(() => cache.Invalidate(key));
            t.ConfigureAwait(false);
            return t;
        }
        public Task RemoveAll()
        {
            var t = Task.Run(() => cache.InvalidateAll());
            t.ConfigureAwait(false);
            return t;
        }
        public Task Set<T>(string key, T value, TimeSpan? expireTimeSpan)
        {
            var t = Task.Run(async () =>
            {
                DateTimeOffset? offset = expireTimeSpan.HasValue ? DateTimeOffset.UtcNow.Add(expireTimeSpan.Value) : (DateTimeOffset?)null;
                await cache.InsertObject(key, value, offset);
            });
            t.ConfigureAwait(false);
            return t;
        }
        #endregion
        
        #region IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    cache = null;
                    BlobCache.Shutdown().Wait();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~BlackPearlCache()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
