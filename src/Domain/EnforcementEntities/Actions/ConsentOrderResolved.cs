using AirWeb.Domain.Identity;

namespace AirWeb.Domain.EnforcementEntities.Actions;

public class ConsentOrderResolved : EnforcementAction
{
    // Constructors
    [UsedImplicitly] // Used by ORM.
    private ConsentOrderResolved() { }

    internal ConsentOrderResolved(Guid id, ConsentOrder consentOrder, ApplicationUser? user)
        : base(id, consentOrder.EnforcementCase, user)
    {
        EnforcementActionType = EnforcementActionType.ConsentOrderResolved;
        ConsentOrder = consentOrder;
    }

    public ConsentOrder ConsentOrder { get; set; } = null!;
}
