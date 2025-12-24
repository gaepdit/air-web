using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Query;

namespace AirWeb.AppServices.Compliance.WorkEntries.Accs;

public record AccViewDto : WorkEntryViewDto
{
    [Display(Name = "Date Received")]
    public DateOnly ReceivedDate { get; init; }

    [Display(Name = "ACC Reporting Year")]
    public int? AccReportingYear { get; init; }

    [Display(Name = "Date Postmarked")]
    public DateOnly? PostmarkDate { get; init; }

    [Display(Name = "Postmarked By Deadline")]
    public bool? PostmarkedOnTime { get; init; }

    [Display(Name = "Signed By Responsible Official")]
    public bool? SignedByRo { get; init; }

    [Display(Name = "Submitted Using Correct ACC Forms")]
    public bool? OnCorrectForms { get; init; }

    [Display(Name = "All Title V Conditions Listed")]
    public bool? IncludesAllTvConditions { get; init; }

    [Display(Name = "Correctly Filled Out")]
    public bool? CorrectlyCompleted { get; init; }

    [Display(Name = "Reported Deviations")]
    public bool? ReportsDeviations { get; init; }

    [Display(Name = "Includes Deviations Not Previously Reported")]
    public bool? IncludesPreviouslyUnreportedDeviations { get; init; }

    [Display(Name = "Includes All Previously Known Deviations")]
    public bool? ReportsAllKnownDeviations { get; init; }

    [Display(Name = "Resubmittal Required")]
    public bool? ResubmittalRequired { get; init; }

    [Display(Name = "Enforcement Needed")]
    public bool? EnforcementNeeded { get; init; }

    public override bool HasPrintout => IsClosed;
    public override string PrintoutPath => "/Print/ACC/Index";
}
