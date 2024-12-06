using AirWeb.Domain.ValueObjects;

namespace AirWeb.Domain.Reports.Compliance;

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
    [UIHint("BooleanYesNo")]
    public bool PostmarkedByDeadline { get; init; }

    [Display(Name = "Certification signed by a responsible official")]
    [UIHint("BooleanYesNo")]
    public bool SignedByResponsibleOfficial { get; init; }

    [Display(Name = "Division's ACC forms used")]
    [UIHint("BooleanYesNo")]
    public bool CorrectFormsUsed { get; init; }

    [Display(Name = "All conditions of the Title V Permit listed")]
    [UIHint("BooleanYesNo")]
    public bool AllTitleVConditionsListed { get; init; }

    [Display(Name = "Initial ACC correctly filled out for each condition in the Title V Permit")]
    [UIHint("BooleanYesNo")]
    public bool CorrectlyFilledOut { get; init; }

    [Display(Name = "The ACC reported deviations")]
    [UIHint("BooleanYesNo")]
    public bool DeviationsReported { get; init; }

    [Display(Name = "In Part 3, the ACC reported deviations that had not previously been reported")]
    [UIHint("BooleanYesNo")]
    public bool UnreportedDeviationsReported { get; init; }

    [Display(Name = "Enforcement recommended based on ACC")]
    [UIHint("BooleanYesNo")]
    public bool EnforcementRecommended { get; init; }

    [Display(Name = "All known deviations were reported")]
    [UIHint("BooleanYesNo")]
    public bool AllDeviationsReported { get; init; }

    [Display(Name = "A resubmittal was requested")]
    [UIHint("BooleanYesNo")]
    public bool ResubmittalRequested { get; init; }
}
