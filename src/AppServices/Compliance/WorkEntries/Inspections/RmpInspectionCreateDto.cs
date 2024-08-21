using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using System.ComponentModel.DataAnnotations;

namespace AirWeb.AppServices.Compliance.WorkEntries.Inspections;

public record RmpInspectionCreateDto : WorkEntryCreateDto, IInspectionCommandDto
{
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    [Display(Name = "Start Date")]
    public DateOnly InspectionStartedDate { get; init; } = DateOnly.FromDateTime(DateTime.Today);

    [DataType(DataType.Time)]
    [Display(Name = "Start Time")]
    public TimeOnly InspectionStartedTime { get; init; } = new(8, 0);

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    [Display(Name = "End Date")]
    public DateOnly InspectionEndedDate { get; init; } = DateOnly.FromDateTime(DateTime.Today);

    [DataType(DataType.Time)]
    [Display(Name = "End Time")]
    public TimeOnly InspectionEndedTime { get; init; } = new(12, 0);

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
