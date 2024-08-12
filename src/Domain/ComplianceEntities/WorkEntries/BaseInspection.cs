using System.ComponentModel;
using System.Text.Json.Serialization;

namespace AirWeb.Domain.ComplianceEntities.WorkEntries;

public abstract class BaseInspection : ComplianceEvent
{
    // Constructors

    [UsedImplicitly] // Used by ORM.
    protected BaseInspection() { }

    protected BaseInspection(int? id) : base(id) { }

    // Properties

    [StringLength(18)]
    public InspectionReason? InspectionReason { get; set; }

    // TODO: split into separate DateOnly and TimeOnly properties
    public DateTime InspectionStarted { get; init; }
    public DateTime InspectionEnded { get; init; }

    [StringLength(250)]
    public string WeatherConditions { get; init; } = string.Empty;

    [StringLength(250)]
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
