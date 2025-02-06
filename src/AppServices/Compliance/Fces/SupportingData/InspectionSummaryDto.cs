using AirWeb.AppServices.Staff.Dto;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.Domain.ValueObjects;

namespace AirWeb.AppServices.Compliance.Fces.SupportingData;

public record InspectionSummaryDto
{
    [Display(Name = "Tracking #")]
    public int Id { get; init; }

    [Display(Name = "Comments:")]
    public string Notes { get; init; } = null!;

    public DateTime InspectionStarted { get; init; }
    public DateTime InspectionEnded { get; init; }

    [Display(Name = "Dates")]
    public DateTimeRange InspectionDateRange => new(InspectionStarted, InspectionEnded);

    [Display(Name = "Reason for Inspection")]
    public InspectionReason? InspectionReason { get; init; }

    [Display(Name = "Facility Operating")]
    public bool FacilityOperating { get; init; }

    [Display(Name = "Status")]
    public bool DeviationsNoted { get; init; }

    [Display(Name = "Inspector")]
    public StaffViewDto? ResponsibleStaff { get; init; }
}
