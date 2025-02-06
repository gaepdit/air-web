using AirWeb.Domain.ValueObjects;

namespace AirWeb.AppServices.Compliance.Fces.SupportingData;

public record SourceTestSummaryDto
{
    [Display(Name = "Tracking #")]
    public int Id { get; init; }

    [Display(Name = "Test Ref #")]
    public int ReferenceNumber { get; init; }

    [Display(Name = "Date received")]
    public DateOnly ReceivedByComplianceDate { get; init; }

    [Display(Name = "Reviewer")]
    public PersonName ResponsibleStaff { get; init; }

    [Display(Name = "Compliance status")]
    public string ComplianceStatus { get; init; } = null!;

    public string PollutantMeasured { get; init; } = null!;

    [Display(Name = "Source tested:")]
    public string SourceTested { get; init; } = null!;
}
