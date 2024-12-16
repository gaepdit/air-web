using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace IaipDataService.Utilities;

internal static class LoggingHelpers
{
    public static void LogCacheHit(this ILogger logger, string cacheKey)
    {
        logger.LogInformation("Cache hit for key: {CacheKey}", cacheKey);
    }

    [SuppressMessage("Usage", "CA2254:Template should be a static expression")]
    public static void LogCacheRefresh(this ILogger logger, string cacheKey, bool forceRefresh = false)
    {
        var message = forceRefresh ? "Forcing cache refresh for key: {CacheKey}" : "Cache miss for key: {CacheKey}";
        logger.LogInformation(message, cacheKey);
    }
}
