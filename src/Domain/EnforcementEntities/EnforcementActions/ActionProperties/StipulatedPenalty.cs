using AirWeb.Domain.DataAttributes;
using AirWeb.Domain.Identity;
using Microsoft.EntityFrameworkCore;

namespace AirWeb.Domain.EnforcementEntities.EnforcementActions.ActionProperties;

public class StipulatedPenalty : AuditableSoftDeleteEntity<Guid>
{
    // Constructors
    [UsedImplicitly] // Used by ORM.
    private StipulatedPenalty() { }

    internal StipulatedPenalty(Guid id, ConsentOrder consentOrder, decimal amount, DateOnly receivedDate,
        ApplicationUser? user)
    {
        Id = id;
        ConsentOrder = consentOrder;
        Amount = amount;
        ReceivedDate = receivedDate;
        SetCreator(user?.Id);
    }

    public ConsentOrder ConsentOrder { get; init; } = null!;

    [Precision(precision: 18, scale: 2)]
    [PositiveDecimal]
    public decimal Amount { get; private init; }

    public DateOnly ReceivedDate { get; private init; }

    [StringLength(7000)]
    public string? Notes { get; set; }
}
