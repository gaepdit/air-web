using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using ZLogger;

namespace AirWeb.AppServices.Core.Caching;

public static class CacheLoggingHelpers
{
    public static readonly EventId AirWebCacheHit = new(2503, nameof(AirWebCacheHit));
    public static readonly EventId AirWebCacheMiss = new(2504, nameof(AirWebCacheMiss));

    extension(ILogger logger)
    {
        private void LogCacheHit(string cacheKey) =>
            logger.ZLogInformation(AirWebCacheHit, $"Cache hit for key: {cacheKey}");

        private void LogCacheMiss(string cacheKey) =>
            logger.ZLogInformation(AirWebCacheMiss, $"Cache miss for key: {cacheKey}");
    }

    extension(HybridCache cache)
    {
        public async Task<TItem> GetOrCreateAsync<TItem>(string key, Func<CancellationToken, Task<TItem>> factory,
            TimeSpan expiration, ILogger logger, string? tag = null, CancellationToken token = default)
        {
            logger.ZLogInformation(AirWebCacheHit, $"Cache search for key: {key}");
            IEnumerable<string>? tags = string.IsNullOrEmpty(tag) ? null : [tag];

            return await cache.GetOrCreateAsync(key, factory: async ct =>
            {
                logger.ZLogInformation(AirWebCacheMiss, $"Cache miss for key: {key}");
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
            logger.LogCacheHit(key);
            return true;
        }

        public void RemoveAll(string[] keys)
        {
            foreach (var key in keys) cache.Remove(key);
        }
    }
}
