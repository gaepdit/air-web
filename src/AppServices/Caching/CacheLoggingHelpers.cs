using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using ZLogger;

namespace AirWeb.AppServices.Caching;

internal static class CacheLoggingHelpers
{
    private static readonly EventId AirWebCacheHit = new(2503, nameof(AirWebCacheHit));
    private static readonly EventId AirWebCacheRefresh = new(2504, nameof(AirWebCacheRefresh));

    extension(ILogger logger)
    {
        private void LogCacheHit(string cacheKey) =>
            logger.ZLogInformation(AirWebCacheHit, $"Cache hit for key: {cacheKey}");

        private void LogCacheRefresh(string cacheKey) =>
            logger.ZLogInformation(AirWebCacheRefresh, $"Cache miss for key: {cacheKey}");
    }

    extension(IMemoryCache cache)
    {
        public TItem Set<TItem>(string key, TItem value, TimeSpan timeSpan, ILogger logger)
        {
            cache.Set(key, value, new MemoryCacheEntryOptions().SetAbsoluteExpiration(timeSpan));
            logger.LogCacheRefresh(key);
            return value;
        }

        public bool TryGetValue<TItem>(string key, ILogger logger, [NotNullWhen(true)] out TItem? value)
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
    }
}
