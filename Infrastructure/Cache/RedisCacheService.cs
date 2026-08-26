using Application.Common.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Cache
{
    public class RedisCacheService(
        IDistributedCache distributedCache) : ICacheService
    {
        public async Task<string> GetAsync(string key, CancellationToken cancellationToken = default)
        {
            return await distributedCache.GetStringAsync(key, cancellationToken);
        }

        public async Task SetAsync(string key, string value, TimeSpan absoluteExperationRelativeToNow, CancellationToken cancellationToken = default)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = absoluteExperationRelativeToNow
            };

            await distributedCache.SetStringAsync(key, value, options, cancellationToken);
        }
    }
}
