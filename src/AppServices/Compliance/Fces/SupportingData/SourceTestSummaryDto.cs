using AirWeb.Domain.ValueObjects;

namespace AirWeb.AppServices.Compliance.Fces.SupportingData;

public record SourceTestSummaryDto
{
    [Display(Name = "Tracking #")]
    public int Id { get; init; }

    [Display(Name = "Test Ref #")]
    public int ReferenceNumber { get; init; }

    [Display(Name = "Date Received")]
    public DateOnly ReceivedByComplianceDate { get; init; }

    [Display(Name = "Reviewer")]
    public PersonName ResponsibleStaff { get; init; }

    [Display(Name = "Compliance Status")]
    public string ComplianceStatus { get; init; } = null!;

    public string PollutantMeasured { get; init; } = null!;

    [Display(Name = "Source Tested:")]
    public string SourceTested { get; init; } = null!;
}
