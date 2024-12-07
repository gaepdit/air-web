using AirWeb.Domain.ValueObjects;
using IaipDataService.Facilities;

namespace AirWeb.AppServices.Reports.ComplianceDto;

public record AccReport
{
    public int Id { get; init; }

    public Facility? Facility { get; set; }
    public PersonName StaffResponsible { get; set; }

    [Display(Name = "ACC reporting year")]
    public int AccReportingYear { get; init; }

    [Display(Name = "Date initial certification postmarked")]
    public DateTime DatePostmarked { get; init; }

    [Display(Name = "Date initial certification received")]
    public DateTime DateReceived { get; init; }

    [Display(Name = "Date complete")]
    public DateTime? DateComplete { get; init; }

    public string Comments { get; init; } = "";

    // ACC evaluation data

    [Display(Name = "ACC postmarked by the deadline")]
    [UIHint("BoolYesNo")]
    public bool PostmarkedByDeadline { get; init; }

    [Display(Name = "Certification signed by a responsible official")]
    [UIHint("BoolYesNo")]
    public bool SignedByResponsibleOfficial { get; init; }

    [Display(Name = "Division's ACC forms used")]
    [UIHint("BoolYesNo")]
    public bool CorrectFormsUsed { get; init; }

    [Display(Name = "All conditions of the Title V Permit listed")]
    [UIHint("BoolYesNo")]
    public bool AllTitleVConditionsListed { get; init; }

    [Display(Name = "Initial ACC correctly filled out for each condition in the Title V Permit")]
    [UIHint("BoolYesNo")]
    public bool CorrectlyFilledOut { get; init; }

    [Display(Name = "The ACC reported deviations")]
    [UIHint("BoolYesNo")]
    public bool DeviationsReported { get; init; }

    [Display(Name = "In Part 3, the ACC reported deviations that had not previously been reported")]
    [UIHint("BoolYesNo")]
    public bool UnreportedDeviationsReported { get; init; }

    [Display(Name = "Enforcement recommended based on ACC")]
    [UIHint("BoolYesNo")]
    public bool EnforcementRecommended { get; init; }

    [Display(Name = "All known deviations were reported")]
    [UIHint("BoolYesNo")]
    public bool AllDeviationsReported { get; init; }

    [Display(Name = "A resubmittal was requested")]
    [UIHint("BoolYesNo")]
    public bool ResubmittalRequested { get; init; }
}
