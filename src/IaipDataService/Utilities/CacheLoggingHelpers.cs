using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace IaipDataService.Utilities;

internal static partial class CacheLoggingHelpers
{
    [LoggerMessage(EventId = 2601, Level = LogLevel.Information, Message = "Cache hit for key: {CacheKey}")]
    private static partial void LogCacheHit(ILogger logger, string cacheKey);

    [LoggerMessage(EventId = 2602, Level = LogLevel.Information, Message = "Forcing cache refresh for key: {CacheKey}")]
    private static partial void LogCacheRefresh(ILogger logger, string cacheKey);

    [LoggerMessage(EventId = 2603, Level = LogLevel.Information, Message = "Cache miss for key: {CacheKey}")]
    private static partial void LogCacheMiss(ILogger logger, string cacheKey);

    extension(IMemoryCache cache)
    {
        public TItem Set<TItem>(string key, TItem value, TimeSpan timeSpan, ILogger logger, bool forceRefresh = false)
        {
            cache.Set(key, value, new MemoryCacheEntryOptions().SetAbsoluteExpiration(timeSpan));
            if (forceRefresh)
                LogCacheRefresh(logger, key);
            else
                LogCacheMiss(logger, key);
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
            LogCacheHit(logger, key);
            return true;
        }
    }
}
