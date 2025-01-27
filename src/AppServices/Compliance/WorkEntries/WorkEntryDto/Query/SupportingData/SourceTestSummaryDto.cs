using AirWeb.Domain.ValueObjects;

namespace AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Query.SupportingData;

public record SourceTestSummaryDto
{
    public int Id { get; init; }

    [Display(Name = "Test Ref #")]
    public int ReferenceNumber { get; init; }

    [Display(Name = "Date received")]
    public DateOnly ReceivedByComplianceDate { get; init; }

    public PersonName ResponsibleStaff { get; init; }

    [Display(Name = "Compliance status")]
    public string ComplianceStatus { get; init; } = null!;

    [Display(Name = "Pollutant measured")]
    public string PollutantMeasured { get; init; } = null!;

    [Display(Name = "Source tested")]
    public string SourceTested { get; init; } = null!;
}
