using AirWeb.Domain.EnforcementEntities.Cases;
using AirWeb.Domain.Identity;

namespace AirWeb.Domain.EnforcementEntities.Actions;

public class AdministrativeOrder : EnforcementAction
{
    // Constructors
    [UsedImplicitly] // Used by ORM.
    private AdministrativeOrder() { }

    internal AdministrativeOrder(Guid id, EnforcementCase enforcementCase, ApplicationUser? user) :
        base(id, enforcementCase, user)
    {
        EnforcementActionType = EnforcementActionType.AdministrativeOrder;
    }

    public DateOnly? Executed { get; set; }
    public DateOnly? Appealed { get; set; }
    public DateOnly? Resolved { get; set; }
    public AoResolvedLetter? ResolvedLetter { get; set; }
}
