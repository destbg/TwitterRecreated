using System;
using Common;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Common
{
    public class MemoryCacheService : IMemoryCacheService
    {
        private readonly MemoryCache _cache = new MemoryCache(
            new MemoryCacheOptions()
            {
                ExpirationScanFrequency = TimeSpan.FromMinutes(1)
            }
        );

        public T GetCacheValue<T>(string key) =>
            _cache.Get<T>(key);

        public void SetCacheValue<T>(string key, T value) =>
            _cache.Set(key, value, DateTimeOffset.Now.AddMinutes(5));
    }
}
