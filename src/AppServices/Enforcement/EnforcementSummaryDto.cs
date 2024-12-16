using AirWeb.Domain.ValueObjects;

namespace AirWeb.AppServices.Enforcement;

public record EnforcementSummaryDto
{
    [Display(Name = "Tracking #")]
    public int Id { get; init; }

    [Display(Name = "Staff responsible")]
    public PersonName ResponsibleStaff { get; init; }

    [Display(Name = "Date")]
    public DateOnly EnforcementDate { get; init; }

    [Display(Name = "Type")]
    public string EnforcementType { get; init; } = null!;
}
