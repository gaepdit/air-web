using AirWeb.Domain.EnforcementEntities.Cases;
using AirWeb.Domain.Identity;

namespace AirWeb.Domain.EnforcementEntities.Actions;

public class NoFurtherAction : EnforcementAction
{
    // Constructors
    [UsedImplicitly] // Used by ORM.
    private NoFurtherAction() { }

    internal NoFurtherAction(Guid id, EnforcementCase enforcementCase, ApplicationUser? user)
        : base(id, enforcementCase, user) =>
        EnforcementActionType = EnforcementActionType.NoFurtherAction;
}
