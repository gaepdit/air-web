namespace AirWeb.AppServices.Core.Caching;

internal static class CacheConstants
{
    public static TimeSpan LookupsCacheTime { get; } = TimeSpan.FromDays(2);
}
