namespace AirWeb.Domain.Entities.WorkEntries;

public class RmpInspection : BaseComplianceEvent
{
    // Constructors

    [UsedImplicitly] // Used by ORM.
    private RmpInspection() { }

    internal RmpInspection(int? id) : base(id)
    {
        WorkEntryType = WorkEntryType.ComplianceEvent;
        ComplianceEventType = ComplianceEventType.RmpInspection;
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
