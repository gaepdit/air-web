namespace IaipDataService;

public static class IaipDataConstants
{
    // Organization
    public const string EpdDirector = "A. Director";

    // Stack Tests
    public const string ConfidentialInfoPlaceholder = "--Conf--";

    // Caching
    public static TimeSpan FacilityDataExpiration { get; } = TimeSpan.FromDays(1);
    public static TimeSpan FacilityListExpiration { get; } = TimeSpan.FromDays(1);
    public static TimeSpan SourceTestSummaryExpiration { get; } = TimeSpan.FromDays(1);
    public static TimeSpan SourceTestListExpiration { get; } = TimeSpan.FromDays(1);
}
