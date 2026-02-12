using AirWeb.AppServices.Staff.Dto;
using AirWeb.Core.Entities.ValueObjects;
using AirWeb.Domain.ComplianceEntities.ComplianceMonitoring;

namespace AirWeb.AppServices.Compliance.Fces.SupportingData;

public record InspectionSummaryDto : BaseSummaryDto
{
    public DateTime InspectionStarted { get; init; }
    public DateTime InspectionEnded { get; init; }

    [Display(Name = "Dates")]
    public DateTimeRange InspectionDateRange => new(InspectionStarted, InspectionEnded);

    [Display(Name = "Reason For Inspection")]
    public InspectionReason? InspectionReason { get; init; }

    [Display(Name = "Facility Operating")]
    public bool FacilityOperating { get; init; }

    [Display(Name = "Status")]
    public bool DeviationsNoted { get; init; }

    [Display(Name = "Inspector")]
    public StaffViewDto? ResponsibleStaff { get; init; }
}
