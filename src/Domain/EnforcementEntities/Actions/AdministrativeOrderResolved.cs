using AirWeb.Domain.Identity;

namespace AirWeb.Domain.EnforcementEntities.Actions;

public class AdministrativeOrderResolved : EnforcementAction
{
    // Constructors
    [UsedImplicitly] // Used by ORM.
    private AdministrativeOrderResolved() { }

    internal AdministrativeOrderResolved(Guid id, AdministrativeOrder administrativeOrder, ApplicationUser? user)
        : base(id, administrativeOrder.EnforcementCase, user)
    {
        EnforcementActionType = EnforcementActionType.AdministrativeOrderResolved;
        AdministrativeOrder = administrativeOrder;
    }

    public AdministrativeOrder AdministrativeOrder { get; set; } = null!;
}
