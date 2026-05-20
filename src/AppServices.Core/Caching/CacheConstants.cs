namespace AirWeb.AppServices.Core.Caching;

internal static class CacheConstants
{
    public static TimeSpan LookupsCacheTime { get; } = TimeSpan.FromDays(5);
    public static TimeSpan StaffServiceCacheTime { get; } = TimeSpan.FromDays(5);
}
