using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto;
using System.ComponentModel.DataAnnotations;

namespace AirWeb.AppServices.Compliance.WorkEntries.Accs;

public record AccUpdateDto : WorkEntryUpdateDto, IAccCommandDto
{
    public bool IsClosed { get; init; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    [Display(Name = "Date Received")]
    public DateOnly ReceivedDate { get; init; }

    [Display(Name = "ACC reporting year")]
    public int AccReportingYear { get; init; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    [Display(Name = "Date postmarked")]
    public DateOnly Postmarked { get; init; }

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

    [Display(Name = "Resubmittal required")]
    public bool ResubmittalRequired { get; init; }

    [Display(Name = "Enforcement needed")]
    public bool EnforcementNeeded { get; init; }
}
