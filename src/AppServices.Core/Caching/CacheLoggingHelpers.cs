using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using ZLogger;

namespace AirWeb.AppServices.Core.Caching;

public static class CacheLoggingHelpers
{
    private static readonly EventId AirWebCacheHit = new(2503, nameof(AirWebCacheHit));
    private static readonly EventId AirWebCacheMiss = new(2504, nameof(AirWebCacheMiss));

    extension(ILogger logger)
    {
        private void LogCacheHit(string cacheKey) =>
            logger.ZLogInformation(AirWebCacheHit, $"Cache hit for key: {cacheKey}");

        private void LogCacheMiss(string cacheKey) =>
            logger.ZLogInformation(AirWebCacheMiss, $"Cache miss for key: {cacheKey}");
    }

    extension(IMemoryCache cache)
    {
        public TItem Set<TItem>(TItem value, string key, TimeSpan timeSpan, ILogger logger)
        {
            cache.Set(key, value, new MemoryCacheEntryOptions().SetAbsoluteExpiration(timeSpan));
            logger.LogCacheMiss(key);
            return value;
        }

        public bool TryGetValue<TItem>(string key, ILogger logger, [NotNullWhen(returnValue: true)] out TItem? value)
        {
            if (!cache.TryGetValue(key, out TItem? result) || result is null || result.Equals(default(TItem)))
            {
                value = default;
                return false;
            }

            value = result;
            logger.LogCacheHit(key);
            return true;
        }

        public void RemoveAll(string[] keys)
        {
            foreach (var key in keys) cache.Remove(key);
        }
    }
}
