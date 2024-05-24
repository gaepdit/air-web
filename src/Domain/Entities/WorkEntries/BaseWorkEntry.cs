using AirWeb.Domain.Entities.Facilities;
using AirWeb.Domain.Identity;
using AirWeb.Domain.ValueObjects;
using System.Text.Json.Serialization;

namespace AirWeb.Domain.Entities.WorkEntries;

public abstract class BaseWorkEntry : AuditableSoftDeleteEntity<int>
{
    // Constructors

    [UsedImplicitly] // Used by ORM.
    private BaseWorkEntry() { }

    private protected BaseWorkEntry(int? id)
    {
        if (id is not null) Id = id.Value;
    }

    // Properties: Basic data
    [StringLength(30)]
    public WorkEntryType WorkEntryType { get; internal init; } = WorkEntryType.Unknown;

    public Facility Facility { get; init; } = default!;
    public ApplicationUser? ResponsibleStaff { get; init; }
    public DateOnly? AcknowledgmentLetterDate { get; init; }

    [StringLength(7000)]
    public string Notes { get; init; } = string.Empty;

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
    Report = 1,
    Inspection = 2,
    SourceTestReview = 3,
    AnnualComplianceCertification = 4,
    Notification = 5,
    RmpInspection = 7,
    PermitRevocation = 8,

#pragma warning disable S1133 : FUTURE: remove after data migration is validated
    [Obsolete("Legacy entry number in legacy database")]
    Reserved = 6,
#pragma warning restore S1133
}
