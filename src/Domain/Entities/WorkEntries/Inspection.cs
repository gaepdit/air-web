using System.ComponentModel;
using System.Text.Json.Serialization;

namespace AirWeb.Domain.Entities.WorkEntries;

public class Inspection : BaseWorkEntry
{
    internal Inspection(int? id) : base(id) => WorkEntryType = WorkEntryType.Inspection;

    public InspectionReason? InspectionReason { get; init; }
    public ComplianceStatus ComplianceStatus { get; init; }
}

// Enums

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum InspectionReason
{
    [Description("Planned Unannounced")] PlannedUnannounced,
    [Description("Planned Announced")] PlannedAnnounced,
    [Description("Unplanned")] Unplanned,
    [Description("Complaint Investigation")] Complaint,
    [Description("Joint EPD/EPA")] JointEpdEpa,
    [Description("Multimedia")] Multimedia,
    [Description("Follow-Up")] FollowUp,
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ComplianceStatus
{
    [Description("In Compliance")] InCompliance,
    [Description("Deviation(s) Noted")] DeviationsNoted,
}
