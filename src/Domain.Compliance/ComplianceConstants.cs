namespace AirWeb.Domain.Compliance;

public static class ComplianceConstants
{
    // Compliance program dates
    public const int EarliestComplianceWorkYear = 2000;
    private static DateOnly EarliestComplianceDate => new(year: EarliestComplianceWorkYear, month: 1, day: 1);
    public static string EarliestComplianceDateHtmlString => EarliestComplianceDate.ToString("yyyy-MM-dd");
}
