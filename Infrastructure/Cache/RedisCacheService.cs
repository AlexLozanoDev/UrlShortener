using Application.Common.Interfaces;
using Infrastructure.Exceptions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Cache
{
    public class RedisCacheService(
        ILogger<RedisCacheService> logger,
        IDistributedCache distributedCache) : ICacheService
    {
        public async Task<string> GetAsync(string key, CancellationToken cancellationToken = default)
        {
            try
            {
                return await distributedCache.GetStringAsync(key, cancellationToken);
            }
            catch (Exception ex) when (ex is RedisConnectionException or RedisTimeoutException)
            {
                //throw new CacheUnvailableException("El servicio de caché (Redis) no está disponible en este momento.", ex);
                logger.LogWarning("El servicio de cache no esta disponible en este momento, {}", ex);
                return null;
            }
        }

        public async Task SetAsync(string key, string value, TimeSpan absoluteExperationRelativeToNow, CancellationToken cancellationToken = default)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = absoluteExperationRelativeToNow
            };

            try
            {
                await distributedCache.SetStringAsync(key, value, options, cancellationToken);
            }
            catch (Exception ex) when (ex is RedisConnectionException or RedisTimeoutException)
            {
                //throw new CacheUnvailableException("El servicio de caché (Redis) no está disponible en este momento.", ex);
                logger.LogError("El servicio de cache no esta disponible en este momento, {}", ex);
            }
        }
    }
}
