using System.Text;
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
            
            if(memoryCache is MemoryCache concreteMemoryCache)
            {
                var cachedKeys  = concreteMemoryCache.Keys.ToList();

                foreach (var key in cachedKeys)
                {
                    memoryCache.Remove(key);
                }

            }
        }

        public string GetDynamicKey(params string[] keyParts)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var part in keyParts)
            {
                sb.Append(part);
            }
            return sb.ToString();
        }

    }
}
