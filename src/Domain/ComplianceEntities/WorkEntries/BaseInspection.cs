using AirWeb.Domain.Identity;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace AirWeb.Domain.ComplianceEntities.WorkEntries;

public abstract class BaseInspection : ComplianceEvent
{
    // Constructors

    [UsedImplicitly] // Used by ORM.
    protected BaseInspection() { }

    protected BaseInspection(int? id, ApplicationUser? user, FacilityId facilityId) : base(id, facilityId)
    {
        Close(user);
    }

    // Properties

    [StringLength(18)]
    public InspectionReason? InspectionReason { get; set; }

    public DateTime InspectionStarted { get; set; }
    public DateTime InspectionEnded { get; set; }

    [StringLength(250)]
    public string WeatherConditions { get; set; } = string.Empty;

    [StringLength(250)]
    public string InspectionGuide { get; set; } = string.Empty;

    public bool FacilityOperating { get; set; }

    // false: "In Compliance"
    // true: "Deviation(s) Noted"
    public bool DeviationsNoted { get; set; }

    public bool FollowupTaken { get; set; }
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
