using AirWeb.AppServices.Compliance.ComplianceWork.WorkEntryDto.Command;
using AirWeb.AppServices.Utilities;
using AirWeb.Domain.ComplianceEntities.ComplianceWork;

namespace AirWeb.AppServices.Compliance.ComplianceWork.Inspections;

public abstract record InspectionCommandDto : WorkEntryCommandDto, IInspectionCommandDto
{
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    [Display(Name = "Start Date")]
    public DateOnly InspectionStartedDate { get; init; } = DateOnly.FromDateTime(DateTime.Today);

    [DataType(DataType.Time)]
    [Display(Name = "Start Time")]
    public TimeOnly InspectionStartedTime { get; init; } = new(8, 0);

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    [Display(Name = "End Date")]
    public DateOnly InspectionEndedDate { get; init; } = DateOnly.FromDateTime(DateTime.Today);

    [DataType(DataType.Time)]
    [Display(Name = "End Time")]
    public TimeOnly InspectionEndedTime { get; init; } = new(16, 0);

    [Display(Name = "Inspection Reason")]
    public InspectionReason InspectionReason { get; init; }

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
