using IaipDataService.SourceTests.Models;

namespace AirWeb.AppServices.Compliance.Compliance.Fces.SupportingData;

public record SourceTestSummaryDto
{
    [Display(Name = "Tracking #")]
    public int Id { get; init; }

    [Display(Name = "Test Reference #")]
    public int? ReferenceNumber { get; init; }

    [Display(Name = "Date Received by Compliance")]
    public DateOnly ReceivedByComplianceDate { get; init; }

    [Display(Name = "Compliance Program Reviewer")]
    public string? ResponsibleStaffSortableFullName { get; init; }

    [Display(Name = "Compliance Determination")]
    public string ComplianceStatus { get; private set; } = null!;

    [Display(Name = "Pollutant Determined")]
    public string PollutantMeasured { get; private set; } = null!;

    [Display(Name = "Source Tested")]
    public string SourceTested { get; private set; } = null!;

    public void AddDetails(SourceTestSummary? summary)
    {
        if (summary is null) return;
        ComplianceStatus = summary.ComplianceStatus;
        PollutantMeasured = summary.Pollutant;
        SourceTested = summary.Source;
    }
}
