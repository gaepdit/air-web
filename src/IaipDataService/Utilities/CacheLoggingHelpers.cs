using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using ZLogger;

namespace IaipDataService.Utilities;

internal static class CacheLoggingHelpers
{
    private static readonly EventId IaipDataCacheHit = new(2601, nameof(IaipDataCacheHit));
    private static readonly EventId IaipDataCacheRefresh = new(2602, nameof(IaipDataCacheRefresh));

    private static void LogCacheHit(this ILogger logger, string cacheKey) =>
        logger.ZLogInformation(IaipDataCacheHit, $"Cache hit for key: {cacheKey}");

    private static void LogCacheRefresh(this ILogger logger, string cacheKey) =>
        logger.ZLogInformation(IaipDataCacheRefresh, $"Forcing cache refresh for key: {cacheKey}");

    private static void LogCacheMiss(this ILogger logger, string cacheKey) =>
        logger.ZLogInformation(IaipDataCacheRefresh, $"Cache miss for key: {cacheKey}");

    public static TItem Set<TItem>(this IMemoryCache cache, string key, TItem value,
        TimeSpan timeSpan, ILogger logger, bool forceRefresh = false)
    {
        cache.Set(key, value, new MemoryCacheEntryOptions().SetAbsoluteExpiration(timeSpan));
        if (forceRefresh)
            logger.LogCacheRefresh(key);
        else
            logger.LogCacheMiss(key);
        return value;
    }

    public static bool TryGetValue<TItem>(this IMemoryCache cache, string key, ILogger logger,
        [NotNullWhen(true)] out TItem? value)
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
