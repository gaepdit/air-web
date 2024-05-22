using System.ComponentModel;
using System.Text.Json.Serialization;

namespace AirWeb.Domain.Entities.WorkEntries;

public class Report : BaseWorkEntry
{
    internal Report(int? id) : base(id) => WorkEntryType = WorkEntryType.Report;

    public ReportingPeriodType ReportingPeriodType { get; init; }
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
