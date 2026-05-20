using AirWeb.Domain.Core.Entities;
using AirWeb.Domain.Sbeap.Entities.ActionItemTypes;
using AirWeb.Domain.Sbeap.Entities.Cases;

namespace AirWeb.Domain.Sbeap.Entities.ActionItems;

public class ActionItem : AuditableSoftDeleteEntity
{
    // Constructors

    [UsedImplicitly] // Used by ORM.
    private ActionItem() { }

    internal ActionItem(Guid id, Casework casework, ActionItemType actionItemType) : base(id)
    {
        Casework = casework;
        ActionItemType = actionItemType;
    }

    // Properties

    public Casework Casework { get; private init; } = null!;
    public ActionItemType ActionItemType { get; set; } = null!;
    public DateOnly ActionDate { get; set; }

    [StringLength(7000)]
    public string Notes { get; set; } = string.Empty;

    public ApplicationUser? EnteredBy { get; set; }
    public DateTimeOffset? EnteredOn { get; set; }
}
