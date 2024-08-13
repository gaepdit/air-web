using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using System.ComponentModel.DataAnnotations;

namespace AirWeb.AppServices.Compliance.WorkEntries.RmpInspections;

public record RmpInspectionViewDto : WorkEntryViewDto
{
    [Display(Name = "Start")]
    public DateTime InspectionStarted { get; init; }

    [Display(Name = "End")]
    public DateTime InspectionEnded { get; init; }

    [Display(Name = "Inspection Reason")]
    public InspectionReason? InspectionReason { get; init; }

    [Display(Name = "Weather Conditions")]
    public string WeatherConditions { get; init; } = string.Empty;

    [Display(Name = "Inspection Guides")]
    public string InspectionGuide { get; init; } = string.Empty;

    [Display(Name = "Facility Operating")]
    public bool FacilityOperating { get; init; }

    [Display(Name = "ComplianceStatus")]
    public bool DeviationsNoted { get; init; }

    [Display(Name = "Follow-up Action Taken")]
    public bool FollowupTaken { get; init; }
}
