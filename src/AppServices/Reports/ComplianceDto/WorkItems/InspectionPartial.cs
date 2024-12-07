using AirWeb.Domain.ValueObjects;

namespace AirWeb.AppServices.Reports.ComplianceDto.WorkItems;

public record InspectionPartial
{
    [Display(Name = "Tracking #")]
    public int Id { get; init; }

    public PersonName Inspector { get; set; }

    [Display(Name = "Date")]
    public DateRange InspectionDate { get; set; }

    [Display(Name = "Reason for inspection")]
    public string Reason { get; init; } = "";

    [Display(Name = "Operating")]
    public bool FacilityWasOperating { get; init; }

    [Display(Name = "Compliance status")]
    public string ComplianceStatus { get; init; } = "";

    public string Comments { get; init; } = "";
}
