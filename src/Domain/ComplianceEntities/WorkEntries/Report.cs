using AirWeb.Domain.Identity;
using System.Text.Json.Serialization;

namespace AirWeb.Domain.ComplianceEntities.WorkEntries;

public class Report : ComplianceEvent
{
    // Constructors

    [UsedImplicitly] // Used by ORM.
    private Report() { }

    internal Report(int? id, FacilityId facilityId, ApplicationUser? user = null)
        : base(id, facilityId, user)
    {
        WorkEntryType = WorkEntryType.Report;
        Close(user);
    }

    // Properties

    public DateOnly ReceivedDate { get; set; }

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

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ReportingPeriodType
{
    [Display(Name = "First Quarter")] FirstQuarter,
    [Display(Name = "Second Quarter")] SecondQuarter,
    [Display(Name = "Third Quarter")] ThirdQuarter,
    [Display(Name = "Fourth Quarter")] FourthQuarter,
    [Display(Name = "First Semiannual")] FirstSemiannual,
    [Display(Name = "Second Semiannual")] SecondSemiannual,
    [Display(Name = "Annual")] Annual,
    [Display(Name = "Other")] Other,
    [Display(Name = "Monthly")] Monthly,
    [Display(Name = "Malfunction/Deviation")] MalfunctionDeviation,
}
