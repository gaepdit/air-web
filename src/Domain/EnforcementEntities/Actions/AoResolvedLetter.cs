using AirWeb.Domain.Identity;

namespace AirWeb.Domain.EnforcementEntities.Actions;

public class AoResolvedLetter : EnforcementAction
{
    // Constructors
    [UsedImplicitly] // Used by ORM.
    private AoResolvedLetter() { }

    internal AoResolvedLetter(Guid id, AdministrativeOrder administrativeOrder, ApplicationUser? user)
        : base(id, administrativeOrder.EnforcementCase, user)
    {
        EnforcementActionType = EnforcementActionType.AdministrativeOrderResolved;
        AdministrativeOrder = administrativeOrder;
    }

    public AdministrativeOrder AdministrativeOrder { get; set; } = null!;
}
