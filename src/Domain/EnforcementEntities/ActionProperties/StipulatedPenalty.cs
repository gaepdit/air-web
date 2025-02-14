using AirWeb.Domain.EnforcementEntities.EnforcementActions;
using AirWeb.Domain.Identity;
using Microsoft.EntityFrameworkCore;

namespace AirWeb.Domain.EnforcementEntities.ActionProperties;

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

    [Precision(12, 2)]
    public decimal Amount { get; set; }

    public DateOnly ReceivedDate { get; set; }

    [StringLength(7000)]
    public string? Notes { get; set; }
}
