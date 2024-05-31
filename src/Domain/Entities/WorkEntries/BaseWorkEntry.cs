using AirWeb.Domain.Entities.Facilities;
using AirWeb.Domain.Identity;
using AirWeb.Domain.ValueObjects;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace AirWeb.Domain.Entities.WorkEntries;

public class BaseWorkEntry : AuditableSoftDeleteEntity<int>
{
    // Constructors

    [UsedImplicitly] // Used by ORM.
    private protected BaseWorkEntry() { }

    private protected BaseWorkEntry(int? id)
    {
        if (id is not null) Id = id.Value;
    }

    // Properties: Basic data
    [StringLength(29)]
    public WorkEntryType WorkEntryType { get; internal init; } = WorkEntryType.Unknown;

    public Facility Facility { get; set; } = default!;
    public ApplicationUser? ResponsibleStaff { get; set; }
    public DateOnly? AcknowledgmentLetterDate { get; set; }

    [StringLength(7000)]
    public string Notes { get; set; } = string.Empty;

    // Properties: Lists
    public List<Comment> Comments { get; } = [];

    // Properties: Closure
    public bool IsClosed { get; internal set; }
    public ApplicationUser? ClosedBy { get; internal set; }
    public DateTimeOffset? ClosedDate { get; internal set; }

    // Properties: Deletion
    public ApplicationUser? DeletedBy { get; set; }

    [StringLength(7000)]
    public string? DeleteComments { get; set; }
}

// Enums

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum WorkEntryType
{
    Unknown = 0,
    Notification = 5,
    [Description("Permit Revocation")] PermitRevocation = 8,
    [Description("Compliance Event")] ComplianceEvent = 9,

    // [Obsolete("Moved to compliance event type")] Report = 1,
    // [Obsolete("Moved to compliance event type")] Inspection = 2,
    // [Obsolete("Moved to compliance event type")] SourceTestReview = 3,
    // [Obsolete("Moved to compliance event type")] AnnualComplianceCertification = 4,
    // [Obsolete("Legacy entry number in legacy database")] Reserved = 6,
    // [Obsolete("Moved to compliance event type")] RmpInspection = 7,
}
