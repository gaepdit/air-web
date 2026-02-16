using AirWeb.Domain.Core.Entities;

namespace AirWeb.Domain.Compliance.ComplianceEntities.ComplianceMonitoring;

public abstract class BaseInspection : ComplianceEvent
{
    // Constructors

    [UsedImplicitly] // Used by ORM.
    protected BaseInspection() { }

    protected BaseInspection(int? id, FacilityId facilityId, ApplicationUser? user)
        : base(id, facilityId, user)
    {
        Close(user);
    }

    // Properties

    [StringLength(18)]
    public InspectionReason? InspectionReason { get; set; }

    public DateTime InspectionStarted
    {
        get;
        set
        {
            field = value;
            EventDate = DateOnly.FromDateTime(value);
        }
    }

    public DateTime InspectionEnded { get; set; }

    [StringLength(250)]
    public string? WeatherConditions { get; set; }

    [StringLength(250)]
    public string? InspectionGuide { get; set; }

    public bool FacilityOperating { get; set; }

    // false: "In Compliance"
    // true: "Deviation(s) Noted"
    public bool DeviationsNoted { get; set; }

    public bool FollowupTaken { get; set; }
}

// Enums

public enum InspectionReason
{
    [Display(Name = "Planned Unannounced")] PlannedUnannounced,
    [Display(Name = "Planned Announced")] PlannedAnnounced,
    [Display(Name = "Unplanned")] Unplanned,
    [Display(Name = "Complaint Investigation")] Complaint,
    [Display(Name = "Joint EPD/EPA")] JointEpdEpa,
    [Display(Name = "Multimedia")] Multimedia,
    [Display(Name = "Follow-Up")] FollowUp,
}
