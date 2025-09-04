using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace AirWeb.AppServices.Caching;

internal static class CacheLoggingHelpers
{
    private static readonly EventId AirWebCacheHit = new(2503, nameof(AirWebCacheHit));
    private static readonly EventId AirWebCacheRefresh = new(2504, nameof(AirWebCacheRefresh));

    private static void LogCacheHit(this ILogger logger, string cacheKey) =>
        logger.LogInformation(AirWebCacheHit, "Cache hit for key: {CacheKey}", cacheKey);

    private static void LogCacheRefresh(this ILogger logger, string cacheKey) =>
        logger.LogInformation(AirWebCacheRefresh, "Cache miss for key: {CacheKey}", cacheKey);

    public static TItem Set<TItem>(this IMemoryCache cache, string key, TItem value,
        TimeSpan timeSpan, ILogger logger)
    {
        cache.Set(key, value, new MemoryCacheEntryOptions().SetAbsoluteExpiration(timeSpan));
        logger.LogCacheRefresh(key);
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
