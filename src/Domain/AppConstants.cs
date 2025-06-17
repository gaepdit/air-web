namespace AirWeb.Domain;

public static class AppConstants
{
    // General purpose
    public const int MinimumNameLength = 2;
    public const int MaximumNameLength = 50;
}

public static class ComplianceConstants
{
    // Compliance program dates
    public const int EarliestWorkEntryYear = 2000;
    private static DateOnly EarliestComplianceDate => new(year: EarliestWorkEntryYear, month: 1, day: 1);
    public static string EarliestComplianceDateHtmlString => EarliestComplianceDate.ToString("yyyy-MM-dd");
}
