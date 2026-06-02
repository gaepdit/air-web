using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using ZLogger;

namespace AirWeb.AppServices.Core.Caching;

public static class CacheLoggingHelpers
{
    private static readonly EventId AirWebCacheSearch = new(2701, nameof(AirWebCacheSearch));
    private static readonly EventId AirWebCacheMiss = new(2702, nameof(AirWebCacheMiss));
    private static readonly EventId AirWebCacheRefresh = new(2703, nameof(AirWebCacheRefresh));

    extension(ILogger logger)
    {
        public void LogCacheSearch(string key) =>
            logger.ZLogInformation(AirWebCacheSearch, $"Cache search for key: {key}");

        public void LogCacheRefresh(string key) =>
            logger.ZLogInformation(AirWebCacheRefresh, $"Cache refresh for key: {key}");

        public void LogCacheMiss(string key) =>
            logger.ZLogInformation(AirWebCacheMiss, $"Cache miss for key: {key}");
    }

    extension(HybridCache cache)
    {
        public async Task<TItem> GetOrCreateAsync<TItem>(string key, Func<CancellationToken, Task<TItem>> factory,
            TimeSpan expiration, ILogger logger, string? tag = null, CancellationToken token = default)
        {
            logger.LogCacheSearch(key);
            IEnumerable<string>? tags = string.IsNullOrEmpty(tag) ? null : [tag];

            return await cache.GetOrCreateAsync(key, factory: async ct =>
            {
                logger.LogCacheMiss(key);
                return await factory(ct).ConfigureAwait(false);
            }, CacheUtilities.GetHybridCacheOptions(expiration), tags, token).ConfigureAwait(false);
        }
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
            logger.LogCacheSearch(key);
            return true;
        }

        public void RemoveAll(string[] keys)
        {
            foreach (var key in keys) cache.Remove(key);
        }
    }
}
