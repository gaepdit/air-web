using AirWeb.Domain.Core.Data.DataAttributes;

namespace AirWeb.Domain.Compliance;

public static class ComplianceConstants
{
    // Compliance program dates
    public const int EarliestComplianceWorkYear = 2000;
    private static DateOnly EarliestComplianceDate => new(year: EarliestComplianceWorkYear, month: 1, day: 1);

    public static string EarliestComplianceDateHtmlString =>
        EarliestComplianceDate.ToString(MaxDateAttribute.HtmlInputDate);

    // The date of the final data migration of SSCP compliance and enforcement data from the IAIP.
    // 28-Feb-2026
    public static readonly DateOnly ComplianceDataMigrationDate = new(year: 2026, month: 2, day: 28);
}
