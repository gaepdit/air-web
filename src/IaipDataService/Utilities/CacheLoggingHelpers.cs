using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace IaipDataService.Utilities;

internal static class CacheLoggingHelpers
{
    private static readonly EventId IaipDataCacheHit = new(2601, nameof(IaipDataCacheHit));
    private static readonly EventId IaipDataCacheRefresh = new(2602, nameof(IaipDataCacheRefresh));

    extension(ILogger logger)
    {
        private void LogCacheHit(string cacheKey) =>
            logger.LogInformation(IaipDataCacheHit, "Cache hit for key: {CacheKey}", cacheKey);

        private void LogCacheRefresh(string cacheKey) =>
            logger.LogInformation(IaipDataCacheRefresh, "Forcing cache refresh for key: {CacheKey}", cacheKey);

        private void LogCacheMiss(string cacheKey) =>
            logger.LogInformation(IaipDataCacheRefresh, "Cache miss for key: {CacheKey}", cacheKey);
    }

    extension(IMemoryCache cache)
    {
        public TItem Set<TItem>(string key, TItem value, TimeSpan timeSpan, ILogger logger, bool forceRefresh = false)
        {
            cache.Set(key, value, new MemoryCacheEntryOptions().SetAbsoluteExpiration(timeSpan));
            if (forceRefresh)
                logger.LogCacheRefresh(key);
            else
                logger.LogCacheMiss(key);
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
