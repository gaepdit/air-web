using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using ZLogger;

namespace IaipDataService.Caching;

internal static class CacheLoggingHelpers
{
    private static readonly EventId IaipDataCacheHit = new(2601, nameof(IaipDataCacheHit));
    private static readonly EventId IaipDataCacheRefresh = new(2602, nameof(IaipDataCacheRefresh));
    private static readonly EventId IaipDataCacheMiss = new(2603, nameof(IaipDataCacheMiss));

    extension(ILogger logger)
    {
        public void LogCacheSearch(string key) =>
            logger.ZLogInformation(IaipDataCacheHit, $"Cache search for key: {key}");

        public void LogCacheRefresh(string key) =>
            logger.ZLogInformation(IaipDataCacheRefresh, $"Cache refresh for key: {key}");

        public void LogCacheMiss(string key) =>
            logger.ZLogInformation(IaipDataCacheMiss, $"Cache miss for key: {key}");
    }
}

public static class CacheUtilities
{
    public static HybridCacheEntryOptions GetHybridCacheOptions(TimeSpan expiration) =>
        new() { Expiration = expiration, LocalCacheExpiration = expiration };
}
