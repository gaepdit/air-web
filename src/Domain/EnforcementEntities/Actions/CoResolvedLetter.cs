using AirWeb.Domain.Identity;

namespace AirWeb.Domain.EnforcementEntities.Actions;

public class CoResolvedLetter : EnforcementAction
{
    // Constructors
    [UsedImplicitly] // Used by ORM.
    private CoResolvedLetter() { }

    internal CoResolvedLetter(Guid id, ConsentOrder consentOrder, ApplicationUser? user)
        : base(id, consentOrder.EnforcementCase, user)
    {
        EnforcementActionType = EnforcementActionType.ConsentOrderResolved;
        ConsentOrder = consentOrder;
    }

    public ConsentOrder ConsentOrder { get; set; } = null!;
}
