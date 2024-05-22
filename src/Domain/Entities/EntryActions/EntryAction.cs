using AirWeb.Domain.Entities.WorkEntries;
using AirWeb.Domain.Identity;

namespace AirWeb.Domain.Entities.EntryActions;

public class EntryAction : AuditableSoftDeleteEntity
{
    // Constructors

    [UsedImplicitly] // Used by ORM.
    private EntryAction() { }

    internal EntryAction(Guid id, BaseWorkEntry baseWorkEntry) : base(id) => BaseWorkEntry = baseWorkEntry;

    // Properties

    public BaseWorkEntry BaseWorkEntry { get; private init; } = default!;

    public DateOnly ActionDate { get; set; }

    [StringLength(10_000)]
    public string Comments { get; set; } = string.Empty;

    // Properties: Deletion

    public ApplicationUser? DeletedBy { get; set; }
}
