using MyApp.Domain.Entities.EntryActions;
using MyApp.Domain.Entities.EntryTypes;
using MyApp.Domain.Identity;
using System.Text.Json.Serialization;

namespace MyApp.Domain.Entities.WorkEntries;

public class WorkEntry : AuditableSoftDeleteEntity
{
    // Constants


    // Constructors

    [UsedImplicitly] // Used by ORM.
    private WorkEntry() { }

    internal WorkEntry(Guid id) : base(id) { }

    // Properties

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
