using AirWeb.Domain.ExternalEntities.Facilities;
using AirWeb.Domain.Identity;
using AirWeb.Domain.ValueObjects;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace AirWeb.Domain.ComplianceEntities.WorkEntries;

public abstract class WorkEntry : AuditableSoftDeleteEntity<int>, IComplianceEntity
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
    public WorkEntryType WorkEntryType { get; internal init; }

    public ApplicationUser? ResponsibleStaff { get; set; }
    public DateOnly? AcknowledgmentLetterDate { get; set; }

    [StringLength(7000)]
    public string Notes { get; set; } = string.Empty;

    // Properties: Lists
    public List<WorkEntryComment> Comments { get; } = [];

    // Compliance Event Properties
    public bool IsComplianceEvent { get; internal init; }

    // FUTURE: Placeholder for managing the EPA data exchange status.
    [JsonIgnore]
    [StringLength(11)]
    public DxStatus EpaDxStatus { get; init; } = DxStatus.NotIncluded;

    // Properties: Closure
    public bool IsClosed { get; internal set; }
    public ApplicationUser? ClosedBy { get; internal set; }
    public DateOnly? ClosedDate { get; internal set; }

    // Properties: Deletion
    public ApplicationUser? DeletedBy { get; set; }

    [StringLength(7000)]
    public string? DeleteComments { get; set; }

    // Calculated properties
    public DateOnly EventDate { get; set; }

    [UsedImplicitly]
    public string EventDateName => WorkEntryType switch
    {
        WorkEntryType.Notification or WorkEntryType.PermitRevocation => "Date Received",
        WorkEntryType.Report or WorkEntryType.AnnualComplianceCertification => "Date Received",
        WorkEntryType.Inspection or WorkEntryType.RmpInspection => "Inspection Date",
        WorkEntryType.SourceTestReview => "Received By Compliance",
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
// Numbering is based on historic values in previous database; may not be needed going forward.
public enum WorkEntryType
{
    [Description("Annual Compliance Certification")] AnnualComplianceCertification = 4,
    Inspection = 2,
    Notification = 5,
    [Description("Permit Revocation")] PermitRevocation = 8,
    Report = 1,
    [Description("RMP Inspection")] RmpInspection = 7,
    [Description("Source Test Review")] SourceTestReview = 3,
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DxStatus
{
    [UsedImplicitly] NotIncluded,
    [UsedImplicitly] Processed,
    [UsedImplicitly] Inserted,
    [UsedImplicitly] Updated,
    [UsedImplicitly] Deleted,
}
