using AirWeb.Domain.Entities.EntryActions;
using AirWeb.Domain.Entities.EntryTypes;
using AirWeb.Domain.Identity;
using System.Text.Json.Serialization;

namespace AirWeb.Domain.Entities.WorkEntries;

public abstract class WorkEntry : AuditableSoftDeleteEntity<int>
{
    // Constants


    // Constructors

    [UsedImplicitly] // Used by ORM.
    private WorkEntry() { }

    private protected WorkEntry(int? id)
    {
        if (id is not null) Id = id.Value;
    }

    // Properties

    [StringLength(30)]
    public WorkEntryType WorkEntryType { get; internal set; } = WorkEntryType.Unknown;

    // Properties: Status & meta-data

    [StringLength(25)]
    public WorkEntryStatus Status { get; internal set; } = WorkEntryStatus.Open;

    public DateTimeOffset ReceivedDate { get; init; } = DateTimeOffset.Now;
    public ApplicationUser? ReceivedBy { get; init; }

    // Properties: Data

    public EntryType? EntryType { get; set; }

    [StringLength(7000)]
    public string Notes { get; set; } = string.Empty;

    // Properties: Actions
    public List<EntryAction> EntryActions { get; } = [];

    // Properties: Closure

    public bool Closed { get; internal set; }
    public ApplicationUser? ClosedBy { get; internal set; }
    public DateTimeOffset? ClosedDate { get; internal set; }

    [StringLength(7000)]
    public string? ClosedComments { get; internal set; }

    // Properties: Deletion

    public ApplicationUser? DeletedBy { get; set; }

    [StringLength(7000)]
    public string? DeleteComments { get; set; }
}

// Enums

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum WorkEntryStatus
{
    Open,
    Closed,
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum WorkEntryType
{
    Unknown = 0,
    Report = 1,
    Inspection = 2,
    PerformanceTestReview = 3,
    AnnualComplianceCertification = 4,
    Notification = 5,
    RmpInspection = 7,
    PermitRevocation = 8,

    [Obsolete("Legacy entry number in legacy database")]
    Reserved = 6,
}
