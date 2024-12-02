using AirWeb.Domain.Identity;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace AirWeb.Domain.ComplianceEntities.WorkEntries;

public class Report : ComplianceEvent
{
    // Constructors

    [UsedImplicitly] // Used by ORM.
    private Report() { }

    internal Report(int? id, ApplicationUser? user, FacilityId facilityId) : base(id, facilityId)
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
    [Description("First Quarter")] FirstQuarter,
    [Description("Second Quarter")] SecondQuarter,
    [Description("Third Quarter")] ThirdQuarter,
    [Description("Fourth Quarter")] FourthQuarter,
    [Description("First Semiannual")] FirstSemiannual,
    [Description("Second Semiannual")] SecondSemiannual,
    [Description("Annual")] Annual,
    [Description("Other")] Other,
    [Description("Monthly")] Monthly,
    [Description("Malfunction/Deviation")] MalfunctionDeviation,
}
