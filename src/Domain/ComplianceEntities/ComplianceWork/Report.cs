using AirWeb.Domain.Identity;

namespace AirWeb.Domain.ComplianceEntities.ComplianceWork;

public class Report : ComplianceEvent
{
    // Constructors

    [UsedImplicitly] // Used by ORM.
    private Report() { }

    internal Report(int? id, FacilityId facilityId, ApplicationUser? user = null)
        : base(id, facilityId, user)
    {
        ComplianceWorkType = ComplianceWorkType.Report;
        Close(user);
    }

    // Properties
    private DateOnly _receivedDate;

    public DateOnly ReceivedDate
    {
        get => _receivedDate;
        set
        {
            _receivedDate = value;
            EventDate = value;
        }
    }

    [StringLength(29)]
    public ReportingPeriodType ReportingPeriodType { get; set; }

    public DateOnly ReportingPeriodStart { get; set; }
    public DateOnly? ReportingPeriodEnd { get; set; }

    [StringLength(500)]
    public string? ReportingPeriodComment { get; set; }

    public DateOnly? DueDate { get; set; }
    public DateOnly? SentDate { get; set; }
    public bool ReportComplete { get; set; }
    public bool ReportsDeviations { get; set; }
    public bool EnforcementNeeded { get; set; }
}

// Enums

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public enum ReportingPeriodType
{
    [Display(Name = "Other")] Other,
    [Display(Name = "Monthly")] Monthly,
    [Display(Name = "First Quarter")] FirstQuarter,
    [Display(Name = "Second Quarter")] SecondQuarter,
    [Display(Name = "Third Quarter")] ThirdQuarter,
    [Display(Name = "Fourth Quarter")] FourthQuarter,
    [Display(Name = "First Semiannual")] FirstSemiannual,
    [Display(Name = "Second Semiannual")] SecondSemiannual,
    [Display(Name = "Annual")] Annual,
    [Display(Name = "Malfunction/Deviation")] MalfunctionDeviation,
}
