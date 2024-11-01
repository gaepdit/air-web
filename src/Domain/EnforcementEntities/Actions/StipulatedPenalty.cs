using AirWeb.Domain.Identity;

namespace AirWeb.Domain.EnforcementEntities.Actions;

public class StipulatedPenalty : AuditableSoftDeleteEntity<Guid>
{
    // Constructors
    [UsedImplicitly] // Used by ORM.
    private StipulatedPenalty() { }

    internal StipulatedPenalty(Guid id, ConsentOrder consentOrder, ApplicationUser? user)
    {
        ConsentOrder = consentOrder;
        SetCreator(user?.Id);
    }

    public ConsentOrder ConsentOrder { get; set; } = null!;

    public decimal StipulatedPenaltyAmount { get; set; }

    [StringLength(7000)]
    public string? StipulatedPenaltyComment { get; set; }

    public short SortOrder { get; set; }
}
