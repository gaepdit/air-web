namespace IaipDataService.Caching;

public static class CacheConstants
{
    // Caching
    public static TimeSpan FacilityExpiration { get; } = TimeSpan.FromDays(1);
    public static TimeSpan FacilityListExpiration { get; } = TimeSpan.FromDays(1);
    public static TimeSpan SourceTestListExpiration { get; } = TimeSpan.FromDays(1);
}
