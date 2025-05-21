using Microsoft.Extensions.Caching.Memory;

namespace EventApi.Interfaces
{
    public interface ICacheHelper
    {
        public MemoryCacheEntryOptions GetDefaultCacheOptions();
        public MemoryCacheEntryOptions GetCacheOptions(TimeSpan slidingExpiration, TimeSpan absoluteExpiration);
        public void RemoveCacheData(IMemoryCache memoryCache);
        public string GetDynamicKey(params string[] keyParts);
    }
}
