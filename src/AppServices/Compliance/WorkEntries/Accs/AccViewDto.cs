using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Query;
using System.ComponentModel.DataAnnotations;

namespace AirWeb.AppServices.Compliance.WorkEntries.Accs;

public record AccViewDto : WorkEntryViewDto
{
    [Display(Name = "Date received")]
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

    public override bool HasPrintout => IsClosed;
    public override string PrintoutUrl => $"https://air.gaepd.org/facility/{FacilityId}/acc-report/{Id}";
}
