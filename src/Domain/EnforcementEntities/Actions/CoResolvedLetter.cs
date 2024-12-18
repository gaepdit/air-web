using AirWeb.Domain.Identity;

namespace AirWeb.Domain.EnforcementEntities.Actions;

public class CoResolvedLetter : EnforcementAction
{
    // Constructors
    [UsedImplicitly] // Used by ORM.
    private CoResolvedLetter() { }

    internal CoResolvedLetter(Guid id, ConsentOrder consentOrder, ApplicationUser? user)
        : base(id, consentOrder.CaseFile, user)
    {
        ActionType = EnforcementActionType.CoResolvedLetter;
        ConsentOrder = consentOrder;
    }

    public ConsentOrder ConsentOrder { get; init; } = null!;
}
