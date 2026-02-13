using AirWeb.AppServices.Compliance.ComplianceMonitoring.ComplianceWorkDto.Command;
using AirWeb.AppServices.Core.Utilities;

namespace AirWeb.AppServices.Compliance.ComplianceMonitoring.Accs;

public abstract record AccCommandDto : ComplianceWorkCommandDto, IAccCommandDto
{
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    [Display(Name = "Date Received")]
    public DateOnly ReceivedDate { get; init; } = DateOnly.FromDateTime(DateTime.Today);

    [Display(Name = "ACC Reporting Year")]
    public int AccReportingYear { get; init; } = DateTime.Today.Year - 1;

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    [Display(Name = "Date Postmarked")]
    public DateOnly PostmarkDate { get; init; } = DateOnly.FromDateTime(DateTime.Today);

    [Display(Name = "Postmarked By Deadline")]
    public bool PostmarkedOnTime { get; init; }

    [Display(Name = "Signed By Responsible Official")]
    public bool SignedByRo { get; init; }

    [Display(Name = "Submitted Using Correct ACC Forms")]
    public bool OnCorrectForms { get; init; }

    [Display(Name = "All Title V Conditions Listed")]
    public bool IncludesAllTvConditions { get; init; }

    [Display(Name = "Correctly Filled Out")]
    public bool CorrectlyCompleted { get; init; }

    [Display(Name = "Reported Deviations")]
    public bool ReportsDeviations { get; init; }

    [Display(Name = "Includes Deviations Not Previously Reported")]
    public bool IncludesPreviouslyUnreportedDeviations { get; init; }

    [Display(Name = "Includes All Previously Known Deviations")]
    public bool ReportsAllKnownDeviations { get; init; }

    [Required]
    [Display(Name = "Resubmittal Required")]
    public bool ResubmittalRequired { get; init; }

    [Display(Name = "Enforcement Needed")]
    public bool EnforcementNeeded { get; init; }
}
