using AirWeb.AppServices.Core.Caching;

namespace AirWeb.WebApp.Platform.AppConfiguration;

internal static class Caching
{
    public static IServiceCollection AddCaching(this IServiceCollection services)
    {
        services.AddHybridCache(options =>
            options.DefaultEntryOptions = CacheUtilities.GetHybridCacheOptions(TimeSpan.FromDays(1)));
        return services;
    }
}
