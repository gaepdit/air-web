using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using System.ComponentModel.DataAnnotations;

namespace AirWeb.AppServices.Compliance.WorkEntries.Inspections;

public record InspectionViewDto : WorkEntryViewDto
{
    [Display(Name = "Start")]
    public DateTime InspectionStarted { get; init; }

    [Display(Name = "End")]
    public DateTime InspectionEnded { get; init; }

    [Display(Name = "Inspection Reason")]
    public InspectionReason? InspectionReason { get; init; }

    [Display(Name = "Weather Conditions")]
    public required string WeatherConditions { get; init; }

    [Display(Name = "Inspection Guides")]
    public required string InspectionGuide { get; init; }

    [Display(Name = "Facility Operating")]
    public bool FacilityOperating { get; init; }

    [Display(Name = "Deviation(s) Noted")]
    public bool DeviationsNoted { get; init; }

    [Display(Name = "Follow-up Action Taken")]
    public bool FollowupTaken { get; init; }
}
