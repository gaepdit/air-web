using AirWeb.Domain.ExternalEntities.Facilities;
using AirWeb.Domain.Identity;
using AirWeb.Domain.ValueObjects;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace AirWeb.Domain.ComplianceEntities.WorkEntries;

public class WorkEntry : AuditableSoftDeleteEntity<int>, IComplianceEntity
{
    // Constructors

    [UsedImplicitly] // Used by ORM.
    private protected WorkEntry() { }

    private protected WorkEntry(int? id)
    {
        if (id is not null) Id = id.Value;
    }

    // Properties: Facility

    [MaxLength(9)]
    public string FacilityId { get; private set; } = string.Empty;

    private Facility _facility = default!;

    [NotMapped]
    public Facility Facility
    {
        get => _facility;
        set
        {
            _facility = value;
            FacilityId = value.Id;
        }
    }

    // Properties: Basic data

    [StringLength(29)]
    public WorkEntryType WorkEntryType { get; internal init; } = WorkEntryType.Unknown;

    public ApplicationUser? ResponsibleStaff { get; set; }
    public DateOnly? AcknowledgmentLetterDate { get; set; }

    [StringLength(7000)]
    public string Notes { get; set; } = string.Empty;

    // Properties: Lists
    public List<WorkEntryComment> Comments { get; } = [];

    // Properties: Closure
    public bool IsClosed { get; internal set; }
    public ApplicationUser? ClosedBy { get; internal set; }

    public DateOnly? ClosedDate { get; internal set; }

    // Properties: Deletion
    public ApplicationUser? DeletedBy { get; set; }

    [StringLength(7000)]
    public string? DeleteComments { get; set; }

    // TPH discriminator
    [StringLength(34)]
    public string WorkType { get; set; }

    // Calculated properties
    public DateOnly EventDate { get; set; }

    [UsedImplicitly]
    public string EventDateName => WorkEntryType switch
    {
        WorkEntryType.Unknown => "Date Created",
        WorkEntryType.Notification or WorkEntryType.PermitRevocation => "Date Received",
        WorkEntryType.ComplianceEvent => (this as ComplianceEvent)!.ComplianceEventType switch
        {
            ComplianceEventType.Unknown => "Date Created",
            ComplianceEventType.Report or ComplianceEventType.AnnualComplianceCertification => "Date Received",
            ComplianceEventType.Inspection or ComplianceEventType.RmpInspection => "Inspection Date",
            ComplianceEventType.SourceTestReview => "Received By Compliance",
            _ => "Error",
        },
        _ => "Error",
    };
}

public record WorkEntryComment : Comment
{
    [UsedImplicitly] // Used by ORM.
    private WorkEntryComment() { }

    private WorkEntryComment(Comment c) : base(c) { }
    public WorkEntryComment(Comment c, int workEntryId) : this(c) => WorkEntryId = workEntryId;
    public int WorkEntryId { get; init; }
}

// Enums

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum WorkEntryType
{
    Unknown = 0,
    Notification = 5,
    [Description("Permit Revocation")] PermitRevocation = 8,
    [Description("Compliance Event")] ComplianceEvent = 9,
}
