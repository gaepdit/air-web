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

    // Calculated properties
    public WorkType WorkType => WorkEntryType switch
    {
        WorkEntryType.Unknown => WorkType.Unknown,
        WorkEntryType.Notification => WorkType.Notification,
        WorkEntryType.PermitRevocation => WorkType.PermitRevocation,
        WorkEntryType.ComplianceEvent => (this as ComplianceEvent)!.ComplianceEventType switch
        {
            ComplianceEventType.Inspection => WorkType.Inspection,
            ComplianceEventType.Unknown => WorkType.Unknown,
            ComplianceEventType.Report => WorkType.Report,
            ComplianceEventType.SourceTestReview => WorkType.SourceTestReview,
            ComplianceEventType.AnnualComplianceCertification => WorkType.AnnualComplianceCertification,
            ComplianceEventType.RmpInspection => WorkType.RmpInspection,
            _ => WorkType.Unknown,
        },
        _ => WorkType.Unknown,
    };

    public DateOnly EventDate => WorkEntryType switch
    {
        WorkEntryType.Unknown => DateOnly.FromDateTime(CreatedAt?.Date ?? DateTime.Today),
        WorkEntryType.Notification => (this as Notification)!.ReceivedDate,
        WorkEntryType.PermitRevocation => (this as PermitRevocation)!.ReceivedDate,
        WorkEntryType.ComplianceEvent => (this as ComplianceEvent)!.ComplianceEventType switch
        {
            ComplianceEventType.Unknown => DateOnly.FromDateTime(CreatedAt?.Date ?? DateTime.Today),
            ComplianceEventType.Report => (this as Report)!.ReceivedDate,
            ComplianceEventType.Inspection => DateOnly.FromDateTime((this as Inspection)!.InspectionStarted),
            ComplianceEventType.SourceTestReview => (this as SourceTestReview)!.ReceivedByCompliance,
            ComplianceEventType.AnnualComplianceCertification => (this as AnnualComplianceCertification)!.ReceivedDate,
            ComplianceEventType.RmpInspection => DateOnly.FromDateTime((this as RmpInspection)!.InspectionStarted),
            _ => DateOnly.MinValue,
        },
        _ => DateOnly.MinValue,
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

public enum WorkType
{
    Unknown,
    Inspection,
    [Description("Annual Compliance Certification")] AnnualComplianceCertification,
    [Description("Permit Revocation")] PermitRevocation,
    Notification,
    Report,
    [Description("RMP Inspection")] RmpInspection,
    [Description("Source Test Review")] SourceTestReview,
}
