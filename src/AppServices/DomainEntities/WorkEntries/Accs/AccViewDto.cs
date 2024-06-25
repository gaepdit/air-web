using AirWeb.AppServices.DomainEntities.WorkEntries.BaseWorkEntryDto;
using System.ComponentModel.DataAnnotations;

namespace AirWeb.AppServices.DomainEntities.WorkEntries.Accs;

public record AccViewDto : BaseWorkEntryViewDto
{
    [Display(Name = "Date Received")]
    public DateOnly ReceivedDate { get; init; }

    [Display(Name = "ACC reporting year")]
    public int AccReportingYear { get; init; }

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
