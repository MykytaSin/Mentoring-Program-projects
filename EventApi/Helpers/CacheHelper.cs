using EventApi.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace EventApi.Helpers
{
    public class CacheHelper : ICacheHelper
    {
        public MemoryCacheEntryOptions GetCacheOptions(TimeSpan slidingExpiration, TimeSpan absoluteExpiration)
        {
            return new MemoryCacheEntryOptions()
                .SetSlidingExpiration(slidingExpiration)
                .SetAbsoluteExpiration(absoluteExpiration);
        }

        public MemoryCacheEntryOptions GetDefaultCacheOptions()
        {
            return new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                .SetAbsoluteExpiration(TimeSpan.FromHours(1));
        }

        public void RemoveCacheData(IMemoryCache memoryCache)
        {
            memoryCache.Remove(Constants.AllEventsCacheKey);
            memoryCache.Remove(Constants.MinimizedEventsCacheKey);
            memoryCache.Remove(Constants.SeatsWithStatusAndPriceCacheKey);
        }

    }
}
