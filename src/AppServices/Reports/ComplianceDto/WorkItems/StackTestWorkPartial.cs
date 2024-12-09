using AirWeb.Domain.ValueObjects;

namespace AirWeb.AppServices.Reports.ComplianceDto.WorkItems;

public record StackTestWorkPartial
{
    [Display(Name = "Tracking #")]
    public int Id { get; init; }

    [Display(Name = "Test Ref #")]
    public int ReferenceNumber { get; init; }

    [Display(Name = "Date received")]
    public DateOnly ReceivedDate { get; init; }

    public PersonName Reviewer { get; init; }

    [Display(Name = "Compliance status")]
    public string ComplianceStatus { get; init; } = "";

    [Display(Name = "Pollutant measured")]
    public string PollutantMeasured { get; init; } = "";

    [Display(Name = "Source tested")]
    public string SourceTested { get; init; } = "";
}
