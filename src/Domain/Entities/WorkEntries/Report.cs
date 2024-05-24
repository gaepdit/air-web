using System.ComponentModel;
using System.Text.Json.Serialization;

namespace AirWeb.Domain.Entities.WorkEntries;

public class Report : BaseComplianceEvent
{
    internal Report(int? id) : base(id)
    {
        WorkEntryType = WorkEntryType.ComplianceEvent;
        ComplianceEventType = ComplianceEventType.Report;
    }

    public DateOnly ReceivedDate { get; init; }

    [StringLength(29)]
    public ReportingPeriodType ReportingPeriodType { get; init; }

    public DateOnly ReportingPeriodStart { get; init; }
    public DateOnly? ReportingPeriodEnd { get; init; }
    public string? ReportingPeriodComment { get; init; }
    public DateOnly? DueDate { get; init; }
    public DateOnly? SentDate { get; init; }
    public bool ReportComplete { get; init; }
    public bool ReportsDeviations { get; init; }
    public bool EnforcementNeeded { get; init; }
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
