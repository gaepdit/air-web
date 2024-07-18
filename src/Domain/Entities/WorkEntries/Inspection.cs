using System.ComponentModel;
using System.Text.Json.Serialization;

namespace AirWeb.Domain.Entities.WorkEntries;

public class Inspection : ComplianceEvent
{
    // Constructors

    [UsedImplicitly] // Used by ORM.
    private Inspection() { }

    internal Inspection(int? id) : base(id)
    {
        WorkEntryType = WorkEntryType.ComplianceEvent;
        ComplianceEventType = ComplianceEventType.Inspection;
        IsClosed = true;
    }

    // Properties

    [StringLength(18)]
    public InspectionReason? InspectionReason { get; set; }

    public DateTime InspectionStarted { get; init; }
    public DateTime InspectionEnded { get; init; }

    // TODO: Limit string length.
    public string WeatherConditions { get; init; } = string.Empty;

    // TODO: Limit string length.
    public string InspectionGuide { get; init; } = string.Empty;

    public bool FacilityOperating { get; init; }

    [StringLength(15)]
    public ComplianceStatus ComplianceStatus { get; set; }

    public bool FollowupTaken { get; init; }
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
