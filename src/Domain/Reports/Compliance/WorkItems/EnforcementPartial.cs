using AirWeb.Domain.ValueObjects;

namespace AirWeb.Domain.Reports.Compliance.WorkItems;

public record EnforcementPartial
{
    [Display(Name = "Tracking #")]
    public int Id { get; init; }

    [Display(Name = "Staff responsible")]
    public PersonName StaffResponsible { get; set; }

    [Display(Name = "Date")]
    public DateTime EnforcementDate { get; init; }

    [Display(Name = "Type")]
    public string EnforcementType { get; init; } = "";
}
