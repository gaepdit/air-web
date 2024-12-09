using AirWeb.Domain.ValueObjects;

namespace AirWeb.AppServices.Reports.ComplianceDto.WorkItems;

public record EnforcementPartial
{
    [Display(Name = "Tracking #")]
    public int Id { get; init; }

    [Display(Name = "Staff responsible")]
    public PersonName StaffResponsible { get; init; }

    [Display(Name = "Date")]
    public DateOnly EnforcementDate { get; init; }

    [Display(Name = "Type")]
    public string EnforcementType { get; init; } = string.Empty;
}
