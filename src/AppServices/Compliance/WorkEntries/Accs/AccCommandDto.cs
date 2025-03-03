using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Command;
using AirWeb.AppServices.Utilities;

namespace AirWeb.AppServices.Compliance.WorkEntries.Accs;

public abstract record AccCommandDto : WorkEntryCommandDto, IAccCommandDto
{
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    [Display(Name = "Date received")]
    public DateOnly ReceivedDate { get; init; } = DateOnly.FromDateTime(DateTime.Today);

    [Display(Name = "ACC reporting year")]
    public int AccReportingYear { get; init; } = DateTime.Today.Year - 1;

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    [Display(Name = "Date postmarked")]
    public DateOnly PostmarkDate { get; init; } = DateOnly.FromDateTime(DateTime.Today);

    [Display(Name = "Postmarked by deadline")]
    public bool PostmarkedOnTime { get; init; }

    [Display(Name = "Signed by responsible official")]
    public bool SignedByRo { get; init; }

    [Display(Name = "Submitted using correct ACC forms")]
    public bool OnCorrectForms { get; init; }

    [Display(Name = "All Title V conditions listed")]
    public bool IncludesAllTvConditions { get; init; }

    [Display(Name = "Correctly filled out")]
    public bool CorrectlyCompleted { get; init; }

    [Display(Name = "Reported deviations")]
    public bool ReportsDeviations { get; init; }

    [Display(Name = "Includes deviations not previously reported")]
    public bool IncludesPreviouslyUnreportedDeviations { get; init; }

    [Display(Name = "Includes all previously known deviations")]
    public bool ReportsAllKnownDeviations { get; init; }

    [Required]
    [Display(Name = "Resubmittal required")]
    public bool ResubmittalRequired { get; init; }

    [Display(Name = "Enforcement needed")]
    public bool EnforcementNeeded { get; init; }
}
