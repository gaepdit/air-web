using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using ZLogger;

namespace IaipDataService.Caching;

internal static class CacheLoggingHelpers
{
    private static readonly EventId IaipCacheSearch = new(2601, nameof(IaipCacheSearch));
    private static readonly EventId IaipCacheRefresh = new(2602, nameof(IaipCacheRefresh));
    private static readonly EventId IaipCacheMiss = new(2603, nameof(IaipCacheMiss));

    extension(ILogger logger)
    {
        public void LogCacheSearch(string key) =>
            logger.ZLogInformation(IaipCacheSearch, $"Cache search for key: {key}");

        public void LogCacheRefresh(string key) =>
            logger.ZLogInformation(IaipCacheRefresh, $"Cache refresh for key: {key}");

        public void LogCacheMiss(string key) =>
            logger.ZLogInformation(IaipCacheMiss, $"Cache miss for key: {key}");
    }
}

public static class CacheUtilities
{
    public static HybridCacheEntryOptions GetHybridCacheOptions(TimeSpan expiration) =>
        new() { Expiration = expiration, LocalCacheExpiration = expiration };
}
