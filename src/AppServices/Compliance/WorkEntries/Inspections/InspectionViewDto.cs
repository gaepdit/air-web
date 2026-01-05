using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Query;
using AirWeb.Domain.ComplianceEntities.ComplianceWork;
using AirWeb.Domain.ValueObjects;

namespace AirWeb.AppServices.Compliance.WorkEntries.Inspections;

public record InspectionViewDto : ComplianceEventViewDto
{
    [Display(Name = "Start")]
    public DateTime InspectionStarted { get; init; }

    [Display(Name = "End")]
    public DateTime InspectionEnded { get; init; }

    [Display(Name = "Date")]
    public DateTimeRange InspectionDateRange => new(InspectionStarted, InspectionEnded);

    [Display(Name = "Inspection Reason")]
    public InspectionReason? InspectionReason { get; init; }

    [Display(Name = "Weather Conditions")]
    public string? WeatherConditions { get; init; }

    [Display(Name = "Inspection Guides")]
    public string? InspectionGuide { get; init; }

    [Display(Name = "Facility Operating")]
    public bool FacilityOperating { get; init; }

    [Display(Name = "Deviation(s) Noted")]
    public bool DeviationsNoted { get; init; }

    [Display(Name = "Follow-up Action Taken")]
    public bool FollowupTaken { get; init; }
}
