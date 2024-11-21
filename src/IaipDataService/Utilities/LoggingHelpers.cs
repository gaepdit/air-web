using Microsoft.Extensions.Logging;

namespace IaipDataService.Utilities;

internal static class LoggingHelpers
{
    public static void LogCacheHit(this ILogger logger, string cacheKey)
    {
        logger.LogInformation("Cache hit for key: {CacheKey}", cacheKey);
    }

    public static void LogCacheRefresh(this ILogger logger, bool forceRefresh, string cacheKey)
    {
        if (forceRefresh)
            logger.LogInformation("Forcing cache refresh for key: {CacheKey}", cacheKey);
        else
            logger.LogInformation("Cache miss for key: {CacheKey}", cacheKey);
    }
}
