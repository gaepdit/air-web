using AirWeb.AppServices.Staff.Dto;

namespace AirWeb.AppServices.Compliance.Fces.SupportingData;

public record EnforcementCaseSummaryDto
{
    [Display(Name = "Tracking #")]
    public int Id { get; init; }

    [Display(Name = "Staff Responsible")]
    public StaffViewDto? ResponsibleStaff { get; init; }

    [Display(Name = "Date")]
    public DateOnly EnforcementDate { get; init; }

    // TODO: Add the following property.
    // [Display(Name = "Status Summary")]
    // public string StatusSummary { get; init; } = null!;
}
